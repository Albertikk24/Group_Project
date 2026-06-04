using BudgetApp.BudgetApp.Strategy;
using BudgetApp.BudgetApp.Controllers;
using BudgetApp.BudgetApp.Infrastructure;
using BudgetApp.BudgetApp.Observers;

namespace BudgetApp.BudgetApp.Views {
  // Представление (MVC - View)
  public class ConsoleView {
    // ========== ПОЛЯ ==========
    private BudgetController _controller;
    private UserNotifier _userNotifier;

    // ========== КОНСТРУКТОР ==========
    public ConsoleView(BudgetController controller, string userName, string userEmail) {
      _controller = controller;
      _userNotifier = new UserNotifier(userName, userEmail);
      _controller.Attach(_userNotifier);
    }

    // ========== ГЛАВНОЕ МЕНЮ ==========
    public void ShowMenu() {
      bool exit = false;

      while (!exit) {
        string menu = "\n" + new string('=', 50) + "\n";
        menu += "        БЮДЖЕТНОЕ ПРИЛОЖЕНИЕ\n";
        menu += new string('=', 50) + "\n";
        menu += "\n 1. Показать бюджет";
        menu += "\n 2. Добавить расход";
        menu += "\n 3. Показать расходы";
        menu += "\n 4. Показать категории";
        menu += "\n 5. Управление сбережениями";
        menu += "\n 6. Сводка по сбережениям";
        menu += "\n 7. Управление лимитами";
        menu += "\n 8. Сгенерировать отчет";
        menu += "\n 9. Экспорт данных";
        menu += "\n 10. Показать логи";
        menu += "\n 0. Выход";
        menu += "\n" + new string('-', 50) + "\n";
        menu += $"Остаток бюджета: {_controller.GetRemainingBudget():C}\n";
        menu += "Выберите действие: ";

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(menu);
        Console.ResetColor();

        string? choice = Console.ReadLine();

        switch (choice) {
          case "1": ShowBudget(); break;
          case "2": AddExpense(); break;
          case "3": ShowExpenses(); break;
          case "4": ShowCategories(); break;
          case "5": ManageSavings(); break;
          case "6": ShowSavingsSummary(); break;
          case "7": ManageLimits(); break;
          case "8": GenerateReport(); break;
          case "9": ManageExport(); break;
          case "10": ShowLogs(); break;
          case "0": exit = true; Console.WriteLine("\nВыход из программы..."); break;
          default: Console.WriteLine("\nНеверный выбор"); WaitForUser(); break;
        }
      }
    }

    // ========== ОТОБРАЖЕНИЕ БЮДЖЕТА ==========
    private void ShowBudget() {
      Console.Clear();
      _controller.ShowBudget();
      WaitForUser();
    }

    // ========== ДОБАВЛЕНИЕ РАСХОДА (ВЫБОР КАТЕГОРИИ ПО НОМЕРУ) ==========
    private void AddExpense() {
      Console.Clear();
      Console.WriteLine("\n=== ДОБАВЛЕНИЕ РАСХОДА ===\n");

      Console.Write("Описание: ");
      string? description = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(description)) {
        Console.WriteLine("Ошибка: описание не может быть пустым");
        WaitForUser();
        return;
      }

      Console.Write("Сумма: ");
      if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0) {
        Console.WriteLine("Ошибка: введите корректную сумму");
        WaitForUser();
        return;
      }

      // Получаем список категорий и показываем с номерами
      var categories = _controller.GetCategoriesWithNumbers();
      
      string categoryList = "\n=== ДОСТУПНЫЕ КАТЕГОРИИ ===\n";
      for (int catIndex = 0; catIndex < categories.Count; ++catIndex) {
        categoryList += $"  {catIndex + 1}. {categories[catIndex]}\n";
      }
      Console.Write(categoryList);
      
      Console.Write("\nВыберите категорию (номер): ");
      if (!int.TryParse(Console.ReadLine(), out int categoryNumber) || categoryNumber < 1 || categoryNumber > categories.Count) {
        Console.WriteLine("Ошибка: неверный номер категории");
        WaitForUser();
        return;
      }

      string selectedCategory = categories[categoryNumber - 1];
      _controller.AddExpense(description, amount, selectedCategory);
      WaitForUser();
    }

    // ========== ОТОБРАЖЕНИЕ РАСХОДОВ ==========
    private void ShowExpenses() {
      Console.Clear();
      _controller.ShowExpenses();
      WaitForUser();
    }

    // ========== ОТОБРАЖЕНИЕ КАТЕГОРИЙ ==========
    private void ShowCategories() {
      Console.Clear();
      var categories = _controller.GetCategoriesWithNumbers();
      
      string output = "\n=== ДОСТУПНЫЕ КАТЕГОРИИ РАСХОДОВ ===\n";
      for (int catIndex = 0; catIndex < categories.Count; ++catIndex) {
        output += $"  {catIndex + 1}. {categories[catIndex]}\n";
      }
      Console.WriteLine(output);
      WaitForUser();
    }

    // ========== УПРАВЛЕНИЕ СБЕРЕЖЕНИЯМИ ==========
    private void ManageSavings() {
      Console.Clear();
      string savingsMenu = "\n=== УПРАВЛЕНИЕ СБЕРЕЖЕНИЯМИ ===\n";
      savingsMenu += " 1. Показать все цели\n";
      savingsMenu += " 2. Создать новую цель\n";
      savingsMenu += " 3. Добавить к цели\n";
      savingsMenu += " 4. Сводка по сбережениям\n";
      savingsMenu += " 0. Назад\n";
      savingsMenu += "Выберите действие: ";

      Console.Write(savingsMenu);
      string? choice = Console.ReadLine();

      switch (choice) {
        case "1":
          _controller.ShowSavings();
          WaitForUser();
          break;
        case "2":
          Console.Write("\nНазвание цели: ");
          string? name = Console.ReadLine();
          if (string.IsNullOrWhiteSpace(name)) {
            Console.WriteLine("Ошибка: название цели не может быть пустым");
            WaitForUser();
            return;
          }

          Console.Write("Целевая сумма: ");
          if (!decimal.TryParse(Console.ReadLine(), out decimal target) || target <= 0) {
            Console.WriteLine("Ошибка: введите корректную положительную сумму");
            WaitForUser();
            return;
          }

          Console.Write("Целевая дата (дд.мм.гггг) или Enter: ");
          string? dateInput = Console.ReadLine();
          DateTime? targetDate = null;
          if (!string.IsNullOrWhiteSpace(dateInput)) {
            if (DateTime.TryParse(dateInput, out DateTime parsedDate)) {
              targetDate = parsedDate;
            } else {
              Console.WriteLine("Ошибка: неверный формат даты. Дата не установлена");
            }
          }

          _controller.CreateSaving(name, target, targetDate);
          WaitForUser();
          break;
        case "3":
          _controller.ShowSavings();
          Console.Write("\nНазвание цели: ");
          string? savingName = Console.ReadLine();
          if (string.IsNullOrWhiteSpace(savingName)) {
            Console.WriteLine("Ошибка: название цели не может быть пустым");
            WaitForUser();
            return;
          }

          Console.Write("Сумма для добавления: ");
          if (!decimal.TryParse(Console.ReadLine(), out decimal addAmount) || addAmount <= 0) {
            Console.WriteLine("Ошибка: введите корректную положительную сумму");
            WaitForUser();
            return;
          }

          _controller.AddToSaving(savingName, addAmount);
          WaitForUser();
          break;
        case "4":
          _controller.ShowSavingsSummary();
          WaitForUser();
          break;
      }
    }

    // ========== СВОДКА ПО СБЕРЕЖЕНИЯМ ==========
    private void ShowSavingsSummary() {
      Console.Clear();
      _controller.ShowSavingsSummary();
      WaitForUser();
    }

    // ========== УПРАВЛЕНИЕ ЛИМИТАМИ (ВЫБОР КАТЕГОРИИ ПО НОМЕРУ) ==========
    private void ManageLimits() {
      Console.Clear();
      string limitsMenu = "\n=== УПРАВЛЕНИЕ ЛИМИТАМИ ===\n";
      limitsMenu += " 1. Показать все лимиты\n";
      limitsMenu += " 2. Установить лимит категории\n";
      limitsMenu += " 3. Проверить лимит категории\n";
      limitsMenu += " 4. Проверить все лимиты\n";
      limitsMenu += " 0. Назад\n";
      limitsMenu += "Выберите действие: ";

      Console.Write(limitsMenu);
      string? choice = Console.ReadLine();

      switch (choice) {
        case "1":
          _controller.ShowAllLimits();
          WaitForUser();
          break;
        case "2":
          // Показать категории с номерами
          var categories = _controller.GetCategoriesWithNumbers();
          string categoryList = "\n=== ДОСТУПНЫЕ КАТЕГОРИИ ===\n";
          for (int catIndex = 0; catIndex < categories.Count; ++catIndex) {
            categoryList += $"  {catIndex + 1}. {categories[catIndex]}\n";
          }
          Console.Write(categoryList);
          
          Console.Write("\nВыберите категорию (номер): ");
          if (!int.TryParse(Console.ReadLine(), out int categoryNumber) || categoryNumber < 1 || categoryNumber > categories.Count) {
            Console.WriteLine("Ошибка: неверный номер категории");
            WaitForUser();
            return;
          }

          string selectedCategory = categories[categoryNumber - 1];
          
          Console.Write("Месячный лимит: ");
          if (!decimal.TryParse(Console.ReadLine(), out decimal limit) || limit <= 0) {
            Console.WriteLine("Ошибка: введите корректный положительный лимит");
            WaitForUser();
            return;
          }

          _controller.SetCategoryLimit(selectedCategory, limit);
          WaitForUser();
          break;
        case "3":
          // Показать категории с номерами
          var limitCategories = _controller.GetCategoriesWithNumbers();
          string limitCategoryList = "\n=== ДОСТУПНЫЕ КАТЕГОРИИ ===\n";
          for (int catIndex = 0; catIndex < limitCategories.Count; ++catIndex) {
            limitCategoryList += $"  {catIndex + 1}. {limitCategories[catIndex]}\n";
          }
          Console.Write(limitCategoryList);
          
          Console.Write("\nВыберите категорию (номер): ");
          if (!int.TryParse(Console.ReadLine(), out int limitCatNumber) || limitCatNumber < 1 || limitCatNumber > limitCategories.Count) {
            Console.WriteLine("Ошибка: неверный номер категории");
            WaitForUser();
            return;
          }

          string selectedLimitCategory = limitCategories[limitCatNumber - 1];
          _controller.ShowLimitByCategory(selectedLimitCategory);
          WaitForUser();
          break;
        case "4":
          _controller.CheckAllLimits();
          WaitForUser();
          break;
      }
    }

    // ========== ГЕНЕРАЦИЯ ОТЧЕТОВ ==========
    private void GenerateReport() {
      Console.Clear();
      string reportMenu = "\n=== ГЕНЕРАЦИЯ ОТЧЕТА ===\n";
      reportMenu += " 1. Месячный отчет\n";
      reportMenu += " 2. Годовой отчет\n";
      reportMenu += " 3. Отчет по категориям\n";
      reportMenu += "Выберите тип отчета: ";

      Console.Write(reportMenu);
      string? choice = Console.ReadLine();

      switch (choice) {
        case "1":
          Console.Write("Введите месяц (1-12): ");
          if (int.TryParse(Console.ReadLine(), out int month) && month >= 1 && month <= 12) {
            _controller.GenerateReport(new MonthlyReportStrategy(new DateTime(DateTime.Now.Year, month, 1)));
          } else {
            Console.WriteLine("Неверный месяц");
          }
          break;
        case "2":
          Console.Write("Введите год: ");
          if (int.TryParse(Console.ReadLine(), out int year) && year > 0) {
            _controller.GenerateReport(new YearlyReportStrategy(year));
          } else {
            Console.WriteLine("Неверный год");
          }
          break;
        case "3":
          _controller.GenerateReport(new CategoryReportStrategy());
          break;
        default:
          Console.WriteLine("Неверный выбор");
          break;
      }

      WaitForUser();
    }

    // ========== ЭКСПОРТ ДАННЫХ ==========
    private void ManageExport() {
      Console.Clear();
      string exportMenu = "\n=== ЭКСПОРТ ДАННЫХ ===\n";
      exportMenu += " 1. Экспорт расходов (CSV)\n";
      exportMenu += " 2. Экспорт бюджета (JSON)\n";
      exportMenu += " 3. Экспорт месячного отчета\n";
      exportMenu += " 4. Экспорт годового отчета\n";
      exportMenu += " 5. Экспорт отчета по категориям\n";
      exportMenu += " 6. Показать файлы экспорта\n";
      exportMenu += " 0. Назад\n";
      exportMenu += "Выберите действие: ";

      Console.Write(exportMenu);
      string? choice = Console.ReadLine();

      switch (choice) {
        case "1":
          _controller.ExportExpenses();
          break;
        case "2":
          _controller.ExportBudget();
          break;
        case "3":
          _controller.ExportReport(new MonthlyReportStrategy(DateTime.Now));
          break;
        case "4":
          _controller.ExportReport(new YearlyReportStrategy(DateTime.Now.Year));
          break;
        case "5":
          _controller.ExportReport(new CategoryReportStrategy());
          break;
        case "6":
          _controller.ShowExportFiles();
          break;
        default:
          return;
      }

      WaitForUser();
    }

    // ========== ПОКАЗ ЛОГОВ ==========
    private void ShowLogs() {
      Console.Clear();
      Logger.Instance.ShowAllLogs();
      WaitForUser();
    }

    // ========== ОЖИДАНИЕ НАЖАТИЯ КЛАВИШИ ==========
    private void WaitForUser() {
      Console.WriteLine("\nНажмите любую клавишу для продолжения...");
      Console.ReadKey();
    }
  }
}