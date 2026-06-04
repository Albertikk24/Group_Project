using BudgetApp.Controllers;
using BudgetApp.Views;
using BudgetApp.Infrastructure;

namespace BudgetApp {
  class Program {
    static void Main(string[] args) {
      // ========== НАСТРОЙКА КОНСОЛИ ==========
      Console.Title = "BudgetApp";

      // ========== ПРИВЕТСТВИЕ ==========
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(new string('=', 50));
      Console.WriteLine("        БЮДЖЕТНОЕ ПРИЛОЖЕНИЕ");
      Console.WriteLine(new string('=', 50));
      Console.ResetColor();

      Logger.Instance.Log("Приложение запущено");

      // ========== ВВОД НАЧАЛЬНОГО ДОХОДА ==========
      Console.Write("\nВведите ваш месячный доход: ");
      if (!decimal.TryParse(Console.ReadLine(), out decimal initialIncome) || initialIncome <= 0) {
        Console.WriteLine("Неверный ввод. Установлен доход по умолчанию: 50000 руб.");
        initialIncome = 50000;
      }

      // ========== СОЗДАНИЕ КОНТРОЛЛЕРА ==========
      BudgetController controller = new BudgetController(initialIncome);

      // ========== ВВОД ДАННЫХ ПОЛЬЗОВАТЕЛЯ ==========
      Console.Write("Введите ваше имя: ");
      string? userName = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(userName)) {
        userName = "Пользователь";
      }

      Console.Write("Введите ваш email: ");
      string? userEmail = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(userEmail)) {
        userEmail = "user@example.com";
      }

      // ========== СОЗДАНИЕ ПРЕДСТАВЛЕНИЯ ==========
      ConsoleView view = new ConsoleView(controller, userName, userEmail);

      Logger.Instance.Log($"Пользователь {userName} авторизован");

      // ========== ЗАПУСК МЕНЮ ==========
      view.ShowMenu();

      Logger.Instance.Log("Приложение завершило работу");
    }
  }
}