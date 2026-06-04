using System.Text;
using System.Text.Json;
using BudgetApp.BudgetApp.Infrastructure;
using BudgetApp.BudgetApp.Models;

namespace BudgetApp.BudgetApp.Services {
  // Сервис для экспорта данных в CSV и JSON
  public static class ExportService {
    private const string EXPORT_FOLDER = "Exports";

    // Статический конструктор - создание папки для экспорта
    static ExportService() {
      if (!Directory.Exists(EXPORT_FOLDER)) {
        Directory.CreateDirectory(EXPORT_FOLDER);
        Logger.Instance.Log($"Создана папка для экспорта: {EXPORT_FOLDER}");
      }
    }

    // ========== ЭКСПОРТ В CSV ==========
    // Экспорт расходов в CSV
    public static void ExportExpensesToCsv(List<Expense> expenses, string? filename = null) {
      if (filename == null) {
        filename = $"expenses_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
      }

      string fullPath = Path.Combine(EXPORT_FOLDER, filename);
      StringBuilder sb = new StringBuilder();

      // Заголовки CSV
      sb.AppendLine("Дата;Описание;Сумма;Категория");

      // Данные
      for (int expenseIndex = 0; expenseIndex < expenses.Count; ++expenseIndex) {
        Expense currentExpense = expenses[expenseIndex];
        sb.AppendLine($"{currentExpense.Date:dd.MM.yyyy};{currentExpense.Description};{currentExpense.Amount};{currentExpense.CategoryName}");
      }

      File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);
      Console.WriteLine($"Экспорт расходов выполнен: {fullPath}");
      Logger.Instance.Log($"Экспорт расходов в CSV: {filename}");
    }

    // Экспорт отчета в CSV
    public static void ExportReportToCsv(string reportContent, string reportType, string? filename = null) {
      if (filename == null) {
        filename = $"{reportType}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
      }

      string fullPath = Path.Combine(EXPORT_FOLDER, filename);
      File.WriteAllText(fullPath, reportContent, Encoding.UTF8);
      Console.WriteLine($"Экспорт отчета выполнен: {fullPath}");
      Logger.Instance.Log($"Экспорт отчета {reportType} в CSV: {filename}");
    }

    // ========== ЭКСПОРТ В JSON ==========
    // Экспорт бюджета в JSON
    public static void ExportBudgetToJson(Budget budget, List<Expense> expenses, List<Saving> savings, string? filename = null) {
      if (filename == null) {
        filename = $"budget_{DateTime.Now:yyyyMMdd_HHmmss}.json";
      }

      string fullPath = Path.Combine(EXPORT_FOLDER, filename);

      // Формирование объекта для экспорта
      var exportData = new {
        Budget = new {
          budget.TotalIncome,
          budget.TotalExpenses,
          budget.RemainingBudget,
          budget.CreatedAt
        },
        Expenses = expenses,
        Savings = savings,
        ExportDate = DateTime.Now
      };

      // Сериализация в JSON с отступами
      string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(fullPath, json, Encoding.UTF8);
      Console.WriteLine($"Экспорт бюджета выполнен: {fullPath}");
      Logger.Instance.Log($"Экспорт бюджета в JSON: {filename}");
    }

    // Показ всех файлов экспорта
    public static void ShowExportFiles() {
      string output = "\n=== ФАЙЛЫ ЭКСПОРТА ===\n";
      string[] files = Directory.GetFiles(EXPORT_FOLDER);

      if (files.Length == 0) {
        output += "Нет файлов экспорта\n";
      } else {
        for (int fileIndex = 0; fileIndex < files.Length; ++fileIndex) {
          string currentFile = files[fileIndex];
          FileInfo info = new FileInfo(currentFile);
          output += $"{Path.GetFileName(currentFile)} - {info.Length} байт - {info.LastWriteTime:dd.MM.yyyy HH:mm}\n";
        }
      }

      Console.WriteLine(output);
    }
  }
}