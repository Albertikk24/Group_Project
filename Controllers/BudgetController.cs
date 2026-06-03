using BudgetApp.Factory;
using BudgetApp.Infrastructure;
using BudgetApp.Models;
using BudgetApp.Services;
using BudgetApp.Strategy;
using groupProject.Infrastructure;
using groupProject.Models;

namespace BudgetApp.Controllers
{
  public class BudgetController
  {
    private Budget _currentBudget;
    private List<Expense> _expenses;
    private List<BudgetLimit> _budgetLimits;
    private List<IBudgetObserver> _observers;
    private List<Saving> _savings;

    public BudgetController()
    {
      _currentBudget = new Budget();
      _expenses = new List<Expense>();
      _budgetLimits = new List<BudgetLimit>();
      _observers = new List<IBudgetObserver>();
      _savings = new List<Saving>();
    }

    // ========== ОСНОВНЫЕ МЕТОДЫ ==========

    // ДОБАВИТЬ ЭТОТ МЕТОД
    public void AddIncome(decimal amount)
    {
      if (amount <= 0)
      {
        Console.WriteLine("Сумма дохода должна быть больше 0!");
        return;
      }

      _currentBudget.TotalIncome += amount;
      _currentBudget.UpdateRemainingBudget();
      Logger.Instance.Log($"Добавлен доход: {amount:C}");
      Notify($"Добавлен доход: {amount:C}",
             _currentBudget.TotalIncome, _currentBudget.RemainingBudget);
      Console.WriteLine($"Доход {amount:C} успешно добавлен!");
    }

    // ДОБАВИТЬ ЭТОТ МЕТОД
    public void AddExpense(string description, decimal amount, string categoryName)
    {
      if (amount <= 0)
      {
        Console.WriteLine("Сумма расхода должна быть больше 0!");
        return;
      }

      if (amount > _currentBudget.RemainingBudget)
      {
        Console.WriteLine($"Недостаточно средств. Доступно: {_currentBudget.RemainingBudget:C}");
        return;
      }

      Expense expense = new Expense(description, amount, categoryName);
      _expenses.Add(expense);
      _currentBudget.TotalExpenses += amount;
      _currentBudget.UpdateRemainingBudget();

      Logger.Instance.Log($"Добавлен расход: {description} - {amount:C} (категория: {categoryName})");
      Notify($"Добавлен расход: {description} ({categoryName})",
             _currentBudget.TotalIncome, _currentBudget.RemainingBudget);

      CheckAllLimits();
      Console.WriteLine($"Расход {amount:C} успешно добавлен!");
    }

    // ДОБАВИТЬ ЭТОТ МЕТОД
    public void ShowBudget()
    {
      string output = "\n=== ТЕКУЩИЙ БЮДЖЕТ ===\n";
      output += $"Доходы: {_currentBudget.TotalIncome:C}\n";
      output += $"Расходы: {_currentBudget.TotalExpenses:C}\n";
      output += $"Остаток: {_currentBudget.RemainingBudget:C}\n";
      Console.WriteLine(output);
    }

    // ДОБАВИТЬ ЭТОТ МЕТОД
    public void ShowExpenses()
    {
      string output = "\n=== ИСТОРИЯ РАСХОДОВ ===\n";
      if (_expenses.Count == 0)
      {
        output += "Расходов пока нет\n";
      }
      else
      {
        foreach (var expense in _expenses)
        {
          output += $"{expense.Date:dd.MM.yyyy} - {expense.Description}: {expense.Amount:C} ({expense.CategoryName})\n";
        }
      }
      Console.WriteLine(output);
    }

    // ДОБАВИТЬ ЭТОТ МЕТОД
    public void ShowCategories()
    {
      Console.WriteLine("\n=== ДОСТУПНЫЕ КАТЕГОРИИ ===");
      var categories = CategoryFactoryProvider.GetAllCategories();

      foreach (var category in categories)
      {
        Console.WriteLine($"  - {category.Name}");
      }
    }

    // ========== МЕТОДЫ ДЛЯ СБЕРЕЖЕНИЙ ==========

    public void ShowSavings()
    {
      string output = "\n=== ЦЕЛИ СБЕРЕЖЕНИЙ ===\n";
      if (_savings.Count == 0)
      {
        output += "Целей сбережений пока нет\n";
      }
      else
      {
        foreach (var saving in _savings)
        {
          output += $"  {saving}\n";
        }
      }
      Console.WriteLine(output);
    }

    public void ShowSavingsSummary()
    {
      decimal totalSaved = 0;
      decimal totalTarget = 0;
      int completedCount = 0;

      foreach (var saving in _savings)
      {
        totalSaved += saving.CurrentAmount;
        totalTarget += saving.TargetAmount;
        if (saving.IsCompleted) completedCount++;
      }

      string output = "\n=== СВОДКА ПО СБЕРЕЖЕНИЯМ ===\n";
      output += $"Всего отложено: {totalSaved:C}\n";
      output += $"Всего целей: {totalTarget:C}\n";
      output += $"Выполнено целей: {completedCount} из {_savings.Count}\n";
      output += $"Общий прогресс: {(totalTarget > 0 ? (totalSaved / totalTarget) * 100 : 0):F0}%\n";
      Console.WriteLine(output);
    }

    public void CreateSaving(string name, decimal targetAmount, DateTime? targetDate = null)
    {
      Saving newSaving = new Saving(name, targetAmount, targetDate);
      _savings.Add(newSaving);
      Logger.Instance.Log($"Создана цель сбережения: {name} - {targetAmount:C}");
      Console.WriteLine($"Цель '{name}' создана!");

      if (targetDate.HasValue)
      {
        Console.WriteLine($"Рекомендуется откладывать: {newSaving.GetMonthlyRecommendation():C} в месяц");
      }
    }

    public void AddToSaving(string savingName, decimal amount)
    {
      Saving? saving = _savings.Find(s => s.Name == savingName);
      if (saving == null)
      {
        Console.WriteLine($"Цель '{savingName}' не найдена");
        return;
      }

      if (amount > _currentBudget.RemainingBudget)
      {
        Console.WriteLine($"Недостаточно средств. Доступно: {_currentBudget.RemainingBudget:C}");
        return;
      }

      saving.AddMoney(amount);
      _currentBudget.TotalExpenses += amount;
      _currentBudget.UpdateRemainingBudget();

      Logger.Instance.Log($"Добавлено {amount:C} к сбережению '{savingName}'");
      Notify($"Добавлено {amount:C} к сбережению '{savingName}'",
             _currentBudget.TotalIncome, _currentBudget.RemainingBudget);

      Console.WriteLine($"Добавлено {amount:C} к цели '{savingName}'");

      if (saving.IsCompleted)
      {
        Console.WriteLine($"\nПОЗДРАВЛЯЕМ! Цель '{savingName}' достигнута!\n");
        Notify($"Цель сбережения '{savingName}' достигнута!",
               _currentBudget.TotalIncome, _currentBudget.RemainingBudget);
      }
    }

    // ========== МЕТОДЫ ДЛЯ ЛИМИТОВ ==========

    public void SetCategoryLimit(string categoryName, decimal monthlyLimit)
    {
      DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

      var existingLimit = _budgetLimits.Find(l =>
          l.CategoryName == categoryName && l.Month == currentMonth);

      if (existingLimit != null)
      {
        existingLimit.MonthlyLimit = monthlyLimit;
        Logger.Instance.Log($"Обновлен лимит для категории {categoryName}: {monthlyLimit:C}");
      }
      else
      {
        BudgetLimit newLimit = new BudgetLimit(categoryName, monthlyLimit, currentMonth);
        _budgetLimits.Add(newLimit);
        Logger.Instance.Log($"Установлен лимит для категории {categoryName}: {monthlyLimit:C}");
      }
    }

    public void ShowAllLimits()
    {
      UpdateLimitSpending();

      string output = "\n=== ЛИМИТЫ КАТЕГОРИЙ ===\n";
      if (_budgetLimits.Count == 0)
      {
        output += "Лимиты не установлены\n";
      }
      else
      {
        foreach (var limit in _budgetLimits)
        {
          output += $"  {limit}\n";
        }
      }
      Console.WriteLine(output);
    }

    public void ShowLimitByCategory(string categoryName)
    {
      UpdateLimitSpending();
      DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

      var limit = _budgetLimits.Find(l =>
          l.CategoryName == categoryName && l.Month == currentMonth);

      if (limit == null)
      {
        Console.WriteLine($"Лимит для категории '{categoryName}' не установлен");
      }
      else
      {
        string output = $"\n=== ЛИМИТ: {categoryName} ===\n";
        output += $"Лимит: {limit.MonthlyLimit:C}\n";
        output += $"Потрачено: {limit.CurrentSpending:C}\n";
        output += $"Осталось: {limit.GetRemainingLimit():C}\n";
        output += $"Статус: {limit.GetWarningLevel()}\n";
        Console.WriteLine(output);
      }
    }

    public void CheckAllLimits()
    {
      UpdateLimitSpending();
      DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

      foreach (var limit in _budgetLimits)
      {
        if (limit.Month == currentMonth && limit.IsExceeded())
        {
          Console.WriteLine($"\nПРЕВЫШЕН ЛИМИТ ПО КАТЕГОРИИ '{limit.CategoryName}'!");
          Console.WriteLine($"Лимит: {limit.MonthlyLimit:C}, потрачено: {limit.CurrentSpending:C}, перерасход: {limit.CurrentSpending - limit.MonthlyLimit:C}\n");
          Notify($"Превышен лимит по категории {limit.CategoryName}",
                 _currentBudget.TotalIncome, _currentBudget.RemainingBudget);
        }
      }
    }

    private void UpdateLimitSpending()
    {
      DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

      foreach (var limit in _budgetLimits)
      {
        if (limit.Month == currentMonth)
        {
          decimal spending = GetCurrentMonthSpendingByCategory(limit.CategoryName);
          limit.CurrentSpending = spending;
        }
      }
    }

    private decimal GetCurrentMonthSpendingByCategory(string categoryName)
    {
      DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

      decimal total = 0;
      foreach (var expense in _expenses)
      {
        if (expense.Date >= startOfMonth && expense.Date <= endOfMonth &&
            expense.CategoryName == categoryName)
        {
          total += expense.Amount;
        }
      }
      return total;
    }

    // ========== МЕТОДЫ ДЛЯ ЭКСПОРТА ==========

    public void ExportExpenses()
    {
      ExportService.ExportExpensesToCsv(_expenses);
    }

    public void ExportBudget()
    {
      ExportService.ExportBudgetToJson(_currentBudget, _expenses, _savings);
    }

    public void ExportReport(IReportStrategy strategy)
    {
      string report = strategy.GenerateReport(_currentBudget, _expenses);
      Console.WriteLine(report);
      ExportService.ExportReportToCsv(report, strategy.GetReportType());
    }

    public void ShowExportFiles()
    {
      ExportService.ShowExportFiles();
    }

    // ========== МЕТОДЫ ДЛЯ НАБЛЮДАТЕЛЕЙ (OBSERVER) ==========

    public void Attach(IBudgetObserver observer)
    {
      _observers.Add(observer);
      Logger.Instance.Log("Наблюдатель добавлен");
    }

    public void Detach(IBudgetObserver observer)
    {
      _observers.Remove(observer);
      Logger.Instance.Log("Наблюдатель удален");
    }

    private void Notify(string message, decimal totalIncome, decimal remainingBudget)
    {
      foreach (var observer in _observers)
      {
        observer.Update(message, totalIncome, remainingBudget);
      }
    }
  }
}