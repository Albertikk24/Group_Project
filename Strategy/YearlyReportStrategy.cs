using BudgetApp.Models;
using groupProject.Models;

namespace BudgetApp.Strategy
{
  public class YearlyReportStrategy : IReportStrategy
  {
    private int _year;

    public YearlyReportStrategy(int year)
    {
      _year = year;
    }

    // ========== ДОБАВИТЬ ЭТОТ МЕТОД ==========
    public string GenerateReport(Budget budget, List<Expense> expenses)
    {
      if (expenses == null || expenses.Count == 0)
      {
        return $"\n=== ГОДОВОЙ ОТЧЕТ ЗА {_year} ===\nНет расходов за этот период.\n";
      }

      // Фильтруем расходы за указанный год
      var yearlyExpenses = expenses
          .Where(e => e.Date.Year == _year)
          .ToList();

      if (yearlyExpenses.Count == 0)
      {
        return $"\n=== ГОДОВОЙ ОТЧЕТ ЗА {_year} ===\nНет расходов за {_year} год.\n";
      }

      decimal totalExpenses = yearlyExpenses.Sum(e => e.Amount);

      // Группировка по месяцам
      var monthlyTotals = yearlyExpenses
          .GroupBy(e => e.Date.Month)
          .Select(g => new { Month = g.Key, Total = g.Sum(e => e.Amount), Count = g.Count() })
          .OrderBy(g => g.Month)
          .ToList();

      // Группировка по категориям (топ категорий за год)
      var categoryTotals = yearlyExpenses
          .GroupBy(e => e.CategoryName)
          .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
          .OrderByDescending(g => g.Total)
          .Take(5)  // Топ-5 категорий
          .ToList();

      string report = $"\n=== ГОДОВОЙ ОТЧЕТ ЗА {_year} ===\n";
      report += $"Всего расходов: {totalExpenses:C}\n";
      report += $"Количество операций: {yearlyExpenses.Count}\n";
      report += $"Средний расход в месяц: {(totalExpenses / 12):C}\n";
      report += $"\n--- ПО МЕСЯЦАМ ---\n";

      string[] monthNames = { "Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };

      foreach (var month in monthlyTotals)
      {
        string monthName = monthNames[month.Month - 1];
        report += $"{monthName}: {month.Total:C} ({month.Count} операций)\n";
      }

      report += $"\n--- ТОП-5 КАТЕГОРИЙ РАСХОДОВ ---\n";
      foreach (var category in categoryTotals)
      {
        decimal percentage = (category.Total / totalExpenses) * 100;
        report += $"{category.Category}: {category.Total:C} ({percentage:F1}%)\n";
      }

      if (budget.TotalIncome > 0)
      {
        decimal savingsRate = ((budget.TotalIncome - totalExpenses) / budget.TotalIncome) * 100;
        report += $"\n--- АНАЛИТИКА ---\n";
        report += $"Всего доходов за год: {budget.TotalIncome:C}\n";
        report += $"Сэкономлено: {budget.TotalIncome - totalExpenses:C}\n";
        report += $"Норма сбережения: {savingsRate:F1}%\n";
      }

      return report;
    }

    // ========== ДОБАВИТЬ ЭТОТ МЕТОД (если требуется интерфейсом) ==========
    public string GetReportType()
    {
      return $"yearly_report_{_year}";
    }
  }
}