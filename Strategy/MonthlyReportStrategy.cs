using BudgetApp.Models;

namespace BudgetApp.Strategy {
  // Стратегия: месячный отчет
  public class MonthlyReportStrategy : IReportStrategy {
    private DateTime _month;

    public MonthlyReportStrategy(DateTime month) {
      _month = month;
    }

    public string GenerateReport(Budget budget, List<Expense> expenses) {
      // Фильтрация расходов за указанный месяц
      List<Expense> monthlyExpenses = new List<Expense>();
      for (int expenseIndex = 0; expenseIndex < expenses.Count; ++expenseIndex) {
        Expense currentExpense = expenses[expenseIndex];
        if (currentExpense.Date.Year == _month.Year && currentExpense.Date.Month == _month.Month) {
          monthlyExpenses.Add(currentExpense);
        }
      }

      // Подсчет суммы расходов за месяц
      decimal totalMonthlyExpenses = 0;
      for (int expenseIndex = 0; expenseIndex < monthlyExpenses.Count; ++expenseIndex) {
        totalMonthlyExpenses += monthlyExpenses[expenseIndex].Amount;
      }

      decimal remaining = budget.TotalIncome - totalMonthlyExpenses;

      // Формирование отчета
      string output = "\n" + new string('=', 50) + "\n";
      output += $"МЕСЯЧНЫЙ ОТЧЕТ: {_month:MMMM yyyy}\n";
      output += new string('=', 50) + "\n";
      output += $"Доходы: {budget.TotalIncome:C}\n";
      output += $"Расходы: {totalMonthlyExpenses:C}\n";
      output += $"Остаток: {remaining:C}\n";
      output += new string('-', 50) + "\n";

      if (remaining > 0) {
        output += $"Можно отложить: {remaining:C}\n";
      } else if (remaining < 0) {
        output += $"Перерасход бюджета: {-remaining:C}\n";
      } else {
        output += "Бюджет сведен к нулю\n";
      }

      output += new string('=', 50) + "\n";
      return output;
    }

    public string GetReportType() {
      return "Monthly";
    }
  }
}