using BudgetApp.Infrastructure;

namespace BudgetApp {
  class Program {
    static void Main(string[] args) {
      Console.Title = "BudgetApp";
      
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(new string('=', 50));
      Console.WriteLine("        БЮДЖЕТНОЕ ПРИЛОЖЕНИЕ");
      Console.WriteLine(new string('=', 50));
      Console.ResetColor();
      
      Logger.Instance.Log("Приложение запущено");
      
      Console.WriteLine("\nПроект в разработке");
      Console.WriteLine("Ожидайте завершения работы всех участников");
      Console.WriteLine("\nНажмите любую клавишу для выхода...");
      Console.ReadKey();
    }
  }
}