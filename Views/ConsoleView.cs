using BudgetApp.Controllers;
using BudgetApp.Strategy;

namespace BudgetApp.Views
{
  public class ConsoleView
  {
    private BudgetController _controller;

    public ConsoleView(BudgetController controller)
    {
      _controller = controller;
    }

    public void Run()
    {
      ShowMainMenu();
    }

    private void ShowMainMenu()
    {
      while (true)
      {
        Console.Clear();
        Console.WriteLine("=== БЮДЖЕТНОЕ ПРИЛОЖЕНИЕ ===\n");

        string menu = "=== МЕНЮ ===\n";
        menu += " 1. Добавить доход\n";
        menu += " 2. Добавить расход\n";
        menu += " 3. Показать бюджет\n";
        menu += " 4. Показать историю расходов\n";
        menu += "\n 5. Управление сбережениями\n";
        menu += " 6. Сводка по сбережениям\n";
        menu += " 7. Управление лимитами\n";
        menu += " 8. Экспорт данных\n";
        menu += "\n 0. Выход\n";
        menu += "\nВыберите действие: ";

        Console.Write(menu);
        string? choice = Console.ReadLine();

        switch (choice)
        {
          case "1": AddIncomeView(); break;
          case "2": AddExpenseView(); break;
          case "3": ShowBudgetView(); break;
          case "4": ShowExpensesView(); break;
          case "5": ManageSavings(); break;
          case "6": ShowSavingsSummary(); break;
          case "7": ManageLimits(); break;
          case "8": ManageExport(); break;
          case "0": return;
          default:
            Console.WriteLine("Неверный выбор!");
            WaitForUser();
            break;
        }
      }
    }

    // ========== ВСЕ НЕОБХОДИМЫЕ МЕТОДЫ ==========

    private void WaitForUser()
    {
      Console.WriteLine("\nНажмите любую клавишу для продолжения...");
      Console.ReadKey();
    }

    // Метод для добавления дохода
    private void AddIncomeView()
    {
      Console.Clear();
      Console.Write("Сумма дохода: ");
      if (decimal.TryParse(Console.ReadLine(), out decimal amount))
      {
        _controller.AddIncome(amount);
        Console.WriteLine("Доход добавлен!");
      }
      else
      {
        Console.WriteLine("Неверная сумма!");
      }
      WaitForUser();
    }

    // Метод для добавления расхода
    private void AddExpenseView()
    {
      Console.Clear();
      Console.Write("Описание расхода: ");
      string? description = Console.ReadLine();
      Console.Write("Сумма расхода: ");
      if (decimal.TryParse(Console.ReadLine(), out decimal amount))
      {
        _controller.ShowCategories();
        Console.Write("Категория: ");
        string? category = Console.ReadLine();
        _controller.AddExpense(description, amount, category);
        Console.WriteLine("Расход добавлен!");
      }
      else
      {
        Console.WriteLine("Неверная сумма!");
      }
      WaitForUser();
    }

    // Метод для показа бюджета
    private void ShowBudgetView()
    {
      Console.Clear();
      _controller.ShowBudget();
      WaitForUser();
    }

    // Метод для показа истории расходов
    private void ShowExpensesView()
    {
      Console.Clear();
      _controller.ShowExpenses();
      WaitForUser();
    }

    // ========== МЕТОДЫ ДЛЯ СБЕРЕЖЕНИЙ ==========

    private void ManageSavings()
    {
      while (true)
      {
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

        switch (choice)
        {
          case "1":
            _controller.ShowSavings();
            WaitForUser();
            break;
          case "2":
            Console.Write("\nНазвание цели: ");
            string? name = Console.ReadLine();
            Console.Write("Целевая сумма: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal target))
            {
              Console.Write("Целевая дата (дд.мм.гггг) или Enter: ");
              DateTime? targetDate = null;
              if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
              {
                targetDate = date;
              }
              _controller.CreateSaving(name, target, targetDate);
            }
            WaitForUser();
            break;
          case "3":
            _controller.ShowSavings();
            Console.Write("\nНазвание цели: ");
            string? savingName = Console.ReadLine();
            Console.Write("Сумма для добавления: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal addAmount))
            {
              _controller.AddToSaving(savingName, addAmount);
            }
            WaitForUser();
            break;
          case "4":
            _controller.ShowSavingsSummary();
            WaitForUser();
            break;
          case "0":
            return;
        }
      }
    }

    // МЕТОД ShowSavingsSummary - ЭТОТ МЕТОД ВЫЗЫВАЕТСЯ ИЗ ГЛАВНОГО МЕНЮ
    private void ShowSavingsSummary()
    {
      Console.Clear();
      _controller.ShowSavingsSummary();
      WaitForUser();
    }

    // ========== МЕТОДЫ ДЛЯ ЛИМИТОВ ==========

    private void ManageLimits()
    {
      while (true)
      {
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

        switch (choice)
        {
          case "1":
            _controller.ShowAllLimits();
            WaitForUser();
            break;
          case "2":
            _controller.ShowCategories();
            Console.Write("\nНазвание категории: ");
            string? category = Console.ReadLine();
            Console.Write("Месячный лимит: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal limit))
            {
              _controller.SetCategoryLimit(category, limit);
              Console.WriteLine("Лимит установлен!");
            }
            WaitForUser();
            break;
          case "3":
            _controller.ShowCategories();
            Console.Write("\nНазвание категории: ");
            string? catName = Console.ReadLine();
            _controller.ShowLimitByCategory(catName);
            WaitForUser();
            break;
          case "4":
            _controller.CheckAllLimits();
            WaitForUser();
            break;
          case "0":
            return;
        }
      }
    }

    // ========== МЕТОДЫ ДЛЯ ЭКСПОРТА ==========

    private void ManageExport()
    {
      while (true)
      {
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

        switch (choice)
        {
          case "1":
            _controller.ExportExpenses();
            WaitForUser();
            break;
          case "2":
            _controller.ExportBudget();
            WaitForUser();
            break;
          case "3":
            _controller.ExportReport(new MonthlyReportStrategy(DateTime.Now));
            WaitForUser();
            break;
          case "4":
            _controller.ExportReport(new YearlyReportStrategy(DateTime.Now.Year));
            WaitForUser();
            break;
          case "5":
            _controller.ExportReport(new CategoryReportStrategy());
            WaitForUser();
            break;
          case "6":
            _controller.ShowExportFiles();
            WaitForUser();
            break;
          case "0":
            return;
        }
      }
    }
  }
}