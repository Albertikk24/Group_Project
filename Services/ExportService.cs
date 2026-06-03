using BudgetApp.Models;
using groupProject.Models;
using System.Text;
using System.Text.Json;

namespace BudgetApp.Services
{
  public static class ExportService
  {
    private static string _exportPath = "Exports";

    static ExportService()
    {
      if (!Directory.Exists(_exportPath))
      {
        Directory.CreateDirectory(_exportPath);
      }
    }

    // Экспорт расходов в CSV
    public static void ExportExpensesToCsv(List<Expense> expenses, string? filename = null)
    {
      if (filename == null)
      {
        filename = $"expenses_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
      }

      string fullPath = Path.Combine(_exportPath, filename);
      StringBuilder sb = new StringBuilder();

      // Заголовки
      sb.AppendLine("Дата;Описание;Сумма;Категория");

      // Данные
      foreach (var expense in expenses)
      {
        sb.AppendLine($"{expense.Date:dd.MM.yyyy};{expense.Description};{expense.Amount};{expense.CategoryName}");
      }

      File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);
      Console.WriteLine($"Экспорт расходов выполнен: {fullPath}");
    }

    // Экспорт отчета в CSV
    public static void ExportReportToCsv(string reportContent, string reportType, string? filename = null)
    {
      if (filename == null)
      {
        filename = $"{reportType}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
      }

      string fullPath = Path.Combine(_exportPath, filename);
      File.WriteAllText(fullPath, reportContent, Encoding.UTF8);
      Console.WriteLine($"Экспорт отчета выполнен: {fullPath}");
    }

    // Экспорт бюджета в JSON
    public static void ExportBudgetToJson(Budget budget, List<Expense> expenses, List<Saving> savings, string? filename = null)
    {
      if (filename == null)
      {
        filename = $"budget_{DateTime.Now:yyyyMMdd_HHmmss}.json";
      }

      string fullPath = Path.Combine(_exportPath, filename);

      var exportData = new
      {
        Budget = new
        {
          budget.TotalIncome,
          budget.TotalExpenses,
          budget.RemainingBudget,
          budget.CreatedAt
        },
        Expenses = expenses,
        Savings = savings,
        ExportDate = DateTime.Now
      };

      string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(fullPath, json, Encoding.UTF8);
      Console.WriteLine($"Экспорт бюджета выполнен: {fullPath}");
    }

    // Показать все файлы экспорта
    public static void ShowExportFiles()
    {
      string output = "\n=== ФАЙЛЫ ЭКСПОРТА ===\n";
      var files = Directory.GetFiles(_exportPath);

      if (files.Length == 0)
      {
        output += "Нет файлов экспорта\n";
      }
      else
      {
        foreach (var file in files)
        {
          FileInfo info = new FileInfo(file);
          output += $"{Path.GetFileName(file)} - {info.Length} байт - {info.LastWriteTime:dd.MM.yyyy HH:mm}\n";
        }
      }
      Console.WriteLine(output);
    }
  }
}