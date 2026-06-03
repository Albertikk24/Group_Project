using System.Collections.Generic;
using System.Linq;
using BudgetApp.Models;

namespace BudgetApp.Strategy {
  // Стратегия: отчет по категориям
  public class CategoryReportStrategy : IReportStrategy {
    public string GenerateReport(Budget budget, List<Expense> expenses) {
      var categoryTotals = expenses
        .GroupBy(e => e.CategoryName)
        .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
        .OrderByDescending(x => x.Total)
        .ToList();

      string output = "\n" + new string('=', 50) + "\n";
      output += "ОТЧЕТ ПО КАТЕГОРИЯМ РАСХОДОВ\n";
      output += new string('=', 50) + "\n";

      foreach (var item in categoryTotals) {
        decimal percentage = budget.TotalExpenses > 0 
          ? (item.Total / budget.TotalExpenses) * 100 
          : 0;
        
        output += $"{item.Category}: {item.Total:C} ({percentage:F1}%)\n";
      }

      output += new string('-', 50) + "\n";
      output += $"ИТОГО РАСХОДОВ: {budget.TotalExpenses:C}\n";
      output += new string('=', 50) + "\n";
      return output;
    }

    public string GetReportType() {
      return "Category";
    }
  }
}