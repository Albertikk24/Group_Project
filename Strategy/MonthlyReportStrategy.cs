using BudgetApp.Models;
using groupProject.Models;

namespace BudgetApp.Strategy
{
  public class MonthlyReportStrategy : IReportStrategy
  {
    private DateTime _month;

    public MonthlyReportStrategy(DateTime month)
    {
      _month = month;
    }

    public string GenerateReport(Budget budget, List<Expense> expenses)
    {
      if (expenses == null || expenses.Count == 0)
      {
        return $"\n=== ОТЧЕТ ЗА {_month:MMMM yyyy} ===\nНет расходов за этот период.\n";
      }

      // Фильтруем расходы за указанный месяц
      var monthlyExpenses = expenses
          .Where(e => e.Date.Year == _month.Year && e.Date.Month == _month.Month)
          .ToList();

      if (monthlyExpenses.Count == 0)
      {
        return $"\n=== ОТЧЕТ ЗА {_month:MMMM yyyy} ===\nНет расходов за {_month:MMMM} {_month.Year}.\n";
      }

      decimal totalExpenses = monthlyExpenses.Sum(e => e.Amount);

      // Группировка по категориям
      var categoryTotals = monthlyExpenses
          .GroupBy(e => e.CategoryName)
          .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount), Count = g.Count() })
          .OrderByDescending(g => g.Total)
          .ToList();

      // Самый большой расход за месяц
      var largestExpense = monthlyExpenses.OrderByDescending(e => e.Amount).FirstOrDefault();

      string report = $"\n=== ОТЧЕТ ЗА {_month:MMMM yyyy} ===\n";
      report += $"Период: {_month:dd.MM.yyyy} - {_month.AddMonths(1).AddDays(-1):dd.MM.yyyy}\n";
      report += $"Всего расходов: {totalExpenses:C}\n";
      report += $"Количество операций: {monthlyExpenses.Count}\n";
      report += $"Средний расход: {(totalExpenses / monthlyExpenses.Count):C}\n";

      if (largestExpense != null)
      {
        report += $"Самый большой расход: {largestExpense.Description} - {largestExpense.Amount:C}\n";
      }

      report += $"\n--- РАСХОДЫ ПО КАТЕГОРИЯМ ---\n";

      foreach (var category in categoryTotals)
      {
        decimal percentage = (category.Total / totalExpenses) * 100;
        report += $"{category.Category}: {category.Total:C} ({percentage:F1}%) - {category.Count} операций\n";
      }

      // Аналитика по бюджету
      report += $"\n--- АНАЛИТИКА БЮДЖЕТА ---\n";
      report += $"Доходы: {budget.TotalIncome:C}\n";
      report += $"Расходы: {totalExpenses:C}\n";
      report += $"Остаток: {budget.TotalIncome - totalExpenses:C}\n";

      if (budget.TotalIncome > 0)
      {
        decimal expenseRate = (totalExpenses / budget.TotalIncome) * 100;
        report += $"Доля расходов от доходов: {expenseRate:F1}%\n";
      }

      return report;
    }

    // ========== ДОБАВИТЬ ЭТОТ МЕТОД (если требуется интерфейсом) ==========
    public string GetReportType()
    {
      return $"monthly_report_{_month:yyyyMM}";
    }
  }
}