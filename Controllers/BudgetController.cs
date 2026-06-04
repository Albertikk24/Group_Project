using BudgetApp.Models;
using BudgetApp.Observers;
using BudgetApp.Factory;
using BudgetApp.Strategy;
using BudgetApp.Services;
using BudgetApp.Infrastructure;

namespace BudgetApp.Controllers {
  // Главный контроллер приложения (MVC - Controller)
  public class BudgetController : ISubject {
    // ========== ПОЛЯ ==========
    private Budget _currentBudget;
    private List<IObserver> _observers;
    private List<Expense> _expenses;
    private List<Saving> _savings;
    private List<Category> _availableCategories;
    private List<BudgetLimit> _budgetLimits;

    // ========== КОНСТРУКТОР ==========
    public BudgetController(decimal initialIncome) {
      _currentBudget = new Budget(initialIncome);
      _observers = new List<IObserver>();
      _expenses = new List<Expense>();
      _savings = new List<Saving>();
      _budgetLimits = new List<BudgetLimit>();

      // Создание категорий через Factory Method
      _availableCategories = CategoryFactoryProvider.CreateAllCategories();

      Logger.Instance.Log($"Контроллер инициализирован. Бюджет: {initialIncome:C}");
    }

    // ========== PATTERN OBSERVER ==========
    public void Attach(IObserver observer) {
      if (observer == null) {
        Logger.Instance.Log("Ошибка: наблюдатель не может быть null");
        return;
      }

      if (!_observers.Contains(observer)) {
        _observers.Add(observer);
        Logger.Instance.Log($"Подписан наблюдатель: {observer.GetObserverName()}");
      }
    }

    public void Detach(IObserver observer) {
      if (observer == null) {
        return;
      }

      _observers.Remove(observer);
      Logger.Instance.Log($"Отписан наблюдатель: {observer.GetObserverName()}");
    }

    public void Notify(string message, decimal currentBudget, decimal remaining) {
      for (int observerIndex = 0; observerIndex < _observers.Count; ++observerIndex) {
        IObserver currentObserver = _observers[observerIndex];
        currentObserver.Update(message, currentBudget, remaining);
      }
    }

    // ========== УПРАВЛЕНИЕ РАСХОДАМИ ==========
    public void AddExpense(string description, decimal amount, string categoryName) {
      // Валидация входных данных
      if (string.IsNullOrWhiteSpace(description)) {
        Console.WriteLine("Ошибка: описание не может быть пустым");
        return;
      }
      if (amount <= 0) {
        Console.WriteLine("Ошибка: сумма должна быть положительной");
        return;
      }
      if (string.IsNullOrWhiteSpace(categoryName)) {
        Console.WriteLine("Ошибка: категория не может быть пустой");
        return;
      }

      // Поиск категории
      Category? category = null;
      for (int categoryIndex = 0; categoryIndex < _availableCategories.Count; ++categoryIndex) {
        if (_availableCategories[categoryIndex].Name == categoryName) {
          category = _availableCategories[categoryIndex];
          break;
        }
      }

      if (category == null) {
        Console.WriteLine($"Категория '{categoryName}' не найдена");
        return;
      }

      // Проверка остатка бюджета
      if (amount > _currentBudget.RemainingBudget) {
        Console.WriteLine($"Недостаточно средств. Доступно: {_currentBudget.RemainingBudget:C}");
        return;
      }

      // Создание и добавление расхода
      Expense newExpense = new Expense(description, amount, categoryName);
      _expenses.Add(newExpense);
      _currentBudget.AddExpense(newExpense);

      Logger.Instance.Log($"Добавлен расход: {description} - {amount:C} ({categoryName})");

      // Обновление лимитов
      UpdateLimitSpending();
      CheckAllLimits();
    }

    // ========== УПРАВЛЕНИЕ ЛИМИТАМИ ==========
    public void SetCategoryLimit(string categoryName, decimal monthlyLimit) {
      if (string.IsNullOrWhiteSpace(categoryName)) {
        Console.WriteLine("Ошибка: категория не может быть пустой");
        return;
      }
      if (monthlyLimit <= 0) {
        Console.WriteLine("Ошибка: лимит должен быть положительным");
        return;
      }

      DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      BudgetLimit? existingLimit = null;

      for (int limitIndex = 0; limitIndex < _budgetLimits.Count; ++limitIndex) {
        BudgetLimit currentLimit = _budgetLimits[limitIndex];
        if (currentLimit.CategoryName == categoryName && currentLimit.Month == currentMonth) {
          existingLimit = currentLimit;
          break;
        }
      }

      if (existingLimit != null) {
        existingLimit.MonthlyLimit = monthlyLimit;
        Logger.Instance.Log($"Обновлен лимит для категории {categoryName}: {monthlyLimit:C}");
      } else {
        BudgetLimit newLimit = new BudgetLimit(categoryName, monthlyLimit, currentMonth);
        _budgetLimits.Add(newLimit);
        Logger.Instance.Log($"Установлен лимит для категории {categoryName}: {monthlyLimit:C}");
      }
    }

    public void UpdateLimitSpending() {
      DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

      for (int limitIndex = 0; limitIndex < _budgetLimits.Count; ++limitIndex) {
        BudgetLimit currentLimit = _budgetLimits[limitIndex];
        if (currentLimit.Month == currentMonth) {
          decimal spending = GetCurrentMonthSpendingByCategory(currentLimit.CategoryName);
          currentLimit.CurrentSpending = spending;
        }
      }
    }

    public void ShowAllLimits() {
      UpdateLimitSpending();

      string output = "\n=== ЛИМИТЫ КАТЕГОРИЙ ===\n";
      if (_budgetLimits.Count == 0) {
        output += "Лимиты не установлены\n";
      } else {
        for (int limitIndex = 0; limitIndex < _budgetLimits.Count; ++limitIndex) {
          output += $"  {_budgetLimits[limitIndex]}\n";
        }
      }
      Console.WriteLine(output);
    }

    public void ShowLimitByCategory(string categoryName) {
      if (string.IsNullOrWhiteSpace(categoryName)) {
        Console.WriteLine("Ошибка: категория не может быть пустой");
        return;
      }

      UpdateLimitSpending();
      DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

      BudgetLimit? foundLimit = null;
      for (int limitIndex = 0; limitIndex < _budgetLimits.Count; ++limitIndex) {
        BudgetLimit currentLimit = _budgetLimits[limitIndex];
        if (currentLimit.CategoryName == categoryName && currentLimit.Month == currentMonth) {
          foundLimit = currentLimit;
          break;
        }
      }

      if (foundLimit == null) {
        Console.WriteLine($"Лимит для категории '{categoryName}' не установлен");
      } else {
        string output = $"\n=== ЛИМИТ: {categoryName} ===\n";
        output += $"Лимит: {foundLimit.MonthlyLimit:C}\n";
        output += $"Потрачено: {foundLimit.CurrentSpending:C}\n";
        output += $"Осталось: {foundLimit.GetRemainingLimit():C}\n";
        output += $"Статус: {foundLimit.GetWarningLevel()}\n";
        Console.WriteLine(output);
      }
    }

    public void CheckAllLimits() {
      UpdateLimitSpending();
      DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

      for (int limitIndex = 0; limitIndex < _budgetLimits.Count; ++limitIndex) {
        BudgetLimit currentLimit = _budgetLimits[limitIndex];
        if (currentLimit.Month == currentMonth && currentLimit.IsExceeded()) {
          string warning = $"\nПРЕВЫШЕН ЛИМИТ ПО КАТЕГОРИИ '{currentLimit.CategoryName}'!";
          warning += $"\nЛимит: {currentLimit.MonthlyLimit:C}, потрачено: {currentLimit.CurrentSpending:C}";
          warning += $"\nПерерасход: {currentLimit.CurrentSpending - currentLimit.MonthlyLimit:C}\n";
          Console.WriteLine(warning);
          Notify($"Превышен лимит по категории {currentLimit.CategoryName}",
                 _currentBudget.TotalIncome, _currentBudget.RemainingBudget);
        }
      }
    }

    // ========== УПРАВЛЕНИЕ СБЕРЕЖЕНИЯМИ ==========
    public void CreateSaving(string name, decimal targetAmount, DateTime? targetDate = null) {
      // Валидация входных данных
      if (string.IsNullOrWhiteSpace(name)) {
        Console.WriteLine("Ошибка: название цели не может быть пустым");
        return;
      }
      if (targetAmount <= 0) {
        Console.WriteLine("Ошибка: целевая сумма должна быть положительной");
        return;
      }

      Saving newSaving = new Saving(name, targetAmount, targetDate);
      _savings.Add(newSaving);
      Logger.Instance.Log($"Создана цель сбережения: {name} - {targetAmount:C}");

      if (targetDate.HasValue) {
        Console.WriteLine($"Рекомендуется откладывать: {newSaving.GetMonthlyRecommendation():C} в месяц");
      }
    }

    public void AddToSaving(string savingName, decimal amount) {
      // Валидация входных данных
      if (string.IsNullOrWhiteSpace(savingName)) {
        Console.WriteLine("Ошибка: название цели не может быть пустым");
        return;
      }
      if (amount <= 0) {
        Console.WriteLine("Ошибка: сумма должна быть положительной");
        return;
      }

      Saving? foundSaving = null;
      for (int savingIndex = 0; savingIndex < _savings.Count; ++savingIndex) {
        if (_savings[savingIndex].Name == savingName) {
          foundSaving = _savings[savingIndex];
          break;
        }
      }

      if (foundSaving == null) {
        Console.WriteLine($"Цель '{savingName}' не найдена");
        return;
      }

      if (amount > _currentBudget.RemainingBudget) {
        Console.WriteLine($"Недостаточно средств. Доступно: {_currentBudget.RemainingBudget:C}");
        return;
      }

      foundSaving.AddMoney(amount);
      _currentBudget.TotalExpenses += amount;
      _currentBudget.UpdateRemainingBudget();

      Logger.Instance.Log($"Добавлено {amount:C} к сбережению '{savingName}'");
      Notify($"Добавлено {amount:C} к сбережению '{savingName}'",
             _currentBudget.TotalIncome, _currentBudget.RemainingBudget);

      if (foundSaving.IsCompleted) {
        Console.WriteLine($"\nПОЗДРАВЛЯЕМ! Цель '{savingName}' достигнута!\n");
        Notify($"Цель сбережения '{savingName}' достигнута!",
               _currentBudget.TotalIncome, _currentBudget.RemainingBudget);
      }
    }

    public void ShowSavings() {
      string output = "\n=== ЦЕЛИ СБЕРЕЖЕНИЙ ===\n";
      if (_savings.Count == 0) {
        output += "Целей сбережений пока нет\n";
      } else {
        for (int savingIndex = 0; savingIndex < _savings.Count; ++savingIndex) {
          Saving currentSaving = _savings[savingIndex];
          output += $"  {currentSaving}\n";
          if (!currentSaving.IsCompleted && currentSaving.TargetDate.HasValue) {
            output += $"     Рекомендуемый ежемесячный взнос: {currentSaving.GetMonthlyRecommendation():C}\n";
          }
        }
      }
      Console.WriteLine(output);
    }

    public void ShowSavingsSummary() {
      decimal totalSaved = 0;
      decimal totalTarget = 0;
      int completedCount = 0;

      for (int savingIndex = 0; savingIndex < _savings.Count; ++savingIndex) {
        Saving currentSaving = _savings[savingIndex];
        totalSaved += currentSaving.CurrentAmount;
        totalTarget += currentSaving.TargetAmount;
        if (currentSaving.IsCompleted) {
          ++completedCount;
        }
      }

      string output = "\n=== СВОДКА ПО СБЕРЕЖЕНИЯМ ===\n";
      output += $"Всего отложено: {totalSaved:C}\n";
      output += $"Всего целей: {totalTarget:C}\n";
      output += $"Выполнено целей: {completedCount} из {_savings.Count}\n";
      output += $"Общий прогресс: {(totalTarget > 0 ? (totalSaved / totalTarget) * 100 : 0):F0}%\n";
      Console.WriteLine(output);
    }

    // ========== ГЕНЕРАЦИЯ ОТЧЕТОВ (STRATEGY PATTERN) ==========
    public void GenerateReport(IReportStrategy strategy) {
      if (strategy == null) {
        Console.WriteLine("Ошибка: стратегия отчета не выбрана");
        return;
      }

      string report = strategy.GenerateReport(_currentBudget, _expenses);
      Console.WriteLine(report);
      Logger.Instance.Log($"Сгенерирован отчет: {strategy.GetReportType()}");
    }

    // ========== ЭКСПОРТ ДАННЫХ ==========
    public void ExportExpenses() {
      ExportService.ExportExpensesToCsv(_expenses);
    }

    public void ExportBudget() {
      ExportService.ExportBudgetToJson(_currentBudget, _expenses, _savings);
    }

    public void ExportReport(IReportStrategy strategy) {
      if (strategy == null) {
        Console.WriteLine("Ошибка: стратегия отчета не выбрана");
        return;
      }

      string report = strategy.GenerateReport(_currentBudget, _expenses);
      ExportService.ExportReportToCsv(report, strategy.GetReportType());
    }

    public void ShowExportFiles() {
      ExportService.ShowExportFiles();
    }

    // ========== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ==========
    private decimal GetCurrentMonthSpendingByCategory(string categoryName) {
      if (string.IsNullOrWhiteSpace(categoryName)) {
        return 0;
      }

      decimal total = 0;
      DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      DateTime nextMonth = currentMonth.AddMonths(1);

      for (int expenseIndex = 0; expenseIndex < _expenses.Count; ++expenseIndex) {
        Expense currentExpense = _expenses[expenseIndex];
        if (currentExpense.CategoryName == categoryName &&
            currentExpense.Date >= currentMonth &&
            currentExpense.Date < nextMonth) {
          total += currentExpense.Amount;
        }
      }
      return total;
    }

    // ========== ПОЛУЧЕНИЕ ДАННЫХ ==========
    public void ShowBudget() {
      Console.WriteLine(_currentBudget.ToString());
    }

    public void ShowExpenses() {
      string output = "\n=== ИСТОРИЯ РАСХОДОВ ===\n";
      if (_expenses.Count == 0) {
        output += "Расходов пока нет\n";
      } else {
        for (int expenseIndex = 0; expenseIndex < _expenses.Count; ++expenseIndex) {
          output += $"  {expenseIndex + 1}. {_expenses[expenseIndex]}\n";
        }
        output += $"\nИТОГО РАСХОДОВ: {_currentBudget.TotalExpenses:C}\n";
      }
      Console.WriteLine(output);
    }

    public void ShowCategories() {
      string output = "\n=== ДОСТУПНЫЕ КАТЕГОРИИ РАСХОДОВ ===\n";
      for (int categoryIndex = 0; categoryIndex < _availableCategories.Count; ++categoryIndex) {
        output += $"  {_availableCategories[categoryIndex]}\n";
      }
      Console.WriteLine(output);
    }

    public decimal GetRemainingBudget() {
      return _currentBudget.RemainingBudget;
    }

    public decimal GetTotalIncome() {
      return _currentBudget.TotalIncome;
    }
  }
}