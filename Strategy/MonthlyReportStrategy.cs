using System;
using System.Collections.Generic;
using System.Linq;
using BudgetApp.Models;

namespace BudgetApp.Strategy {
  // Стратегия: месячный отчет
  public class MonthlyReportStrategy : IReportStrategy {
    private DateTime _month;

    public MonthlyReportStrategy(DateTime month) {
      _month = month;
    }

    public string GenerateReport(Budget budget, List<Expense> expenses) {
      var monthlyExpenses = expenses.Where(e => 
        e.Date.Year == _month.Year && e.Date.Month == _month.Month
      ).ToList();

      decimal totalMonthlyExpenses = monthlyExpenses.Sum(e => e.Amount);
      decimal remaining = budget.TotalIncome - totalMonthlyExpenses;

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