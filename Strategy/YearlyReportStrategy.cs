using System;
using System.Collections.Generic;
using System.Linq;
using BudgetApp.Models;

namespace BudgetApp.Strategy {
  // Стратегия: годовой отчет
  public class YearlyReportStrategy : IReportStrategy {
    private int _year;

    public YearlyReportStrategy(int year) {
      _year = year;
    }

    public string GenerateReport(Budget budget, List<Expense> expenses) {
      var yearlyExpenses = expenses.Where(e => e.Date.Year == _year).ToList();
      decimal totalYearlyExpenses = yearlyExpenses.Sum(e => e.Amount);
      decimal totalYearlyIncome = budget.TotalIncome * 12;
      decimal remaining = totalYearlyIncome - totalYearlyExpenses;
      decimal averageMonthlyExpense = yearlyExpenses.Count > 0 ? totalYearlyExpenses / 12 : 0;

      string output = "\n" + new string('=', 50) + "\n";
      output += $"ГОДОВОЙ ОТЧЕТ: {_year}\n";
      output += new string('=', 50) + "\n";
      output += $"Годовой доход: {totalYearlyIncome:C}\n";
      output += $"Годовые расходы: {totalYearlyExpenses:C}\n";
      output += $"Среднемесячные расходы: {averageMonthlyExpense:C}\n";
      output += $"Остаток за год: {remaining:C}\n";
      output += new string('=', 50) + "\n";
      return output;
    }

    public string GetReportType() {
      return "Yearly";
    }
  }
}