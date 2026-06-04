using BudgetApp.BudgetApp.Models;

namespace BudgetApp.BudgetApp.Strategy {
  // Паттерн Strategy: стратегия отчета по категориям
  public class CategoryReportStrategy : IReportStrategy {
    
    public string GenerateReport(Budget budget, List<Expense> expenses) {
      // Проверка входных данных
      if (budget == null) {
        return "Ошибка: бюджет не может быть null\n";
      }
      if (expenses == null) {
        return "Ошибка: список расходов не может быть null\n";
      }

      // Группировка расходов по категориям
      Dictionary<string, decimal> categoryTotals = new Dictionary<string, decimal>();
      
      for (int expenseIndex = 0; expenseIndex < expenses.Count; ++expenseIndex) {
        Expense currentExpense = expenses[expenseIndex];
        string categoryName = currentExpense.CategoryName;
        
        if (categoryTotals.ContainsKey(categoryName)) {
          categoryTotals[categoryName] += currentExpense.Amount;
        } else {
          categoryTotals[categoryName] = currentExpense.Amount;
        }
      }

      // Формирование отчета (один вывод)
      string output = "\n" + new string('=', 55) + "\n";
      output += "         ОТЧЕТ ПО КАТЕГОРИЯМ РАСХОДОВ\n";
      output += new string('=', 55) + "\n\n";

      if (categoryTotals.Count == 0) {
        output += "  Нет данных о расходах\n";
      } else {
        // Список категорий с суммами и процентами
        foreach (KeyValuePair<string, decimal> item in categoryTotals) {
          decimal percentage = budget.TotalExpenses > 0 
            ? (item.Value / budget.TotalExpenses) * 100 
            : 0;
          
          string categoryLine = string.Format("  {0,-25} : {1,12:C}  ({2,5:F1}%)", 
            item.Key, item.Value, percentage);
          output += categoryLine + "\n";
        }
      }

      output += "\n" + new string('-', 55) + "\n";
      output += string.Format("  {0,-25} : {1,12:C}\n", 
        "ИТОГО РАСХОДОВ", budget.TotalExpenses);
      output += string.Format("  {0,-25} : {1,12:C}\n", 
        "ДОХОД", budget.TotalIncome);
      output += string.Format("  {0,-25} : {1,12:C}\n", 
        "ОСТАТОК", budget.RemainingBudget);
      output += new string('=', 55) + "\n";

      // Добавление рекомендации
      if (budget.RemainingBudget < 0) {
        output += "\n⚠️  ВНИМАНИЕ: Бюджет превышен!\n";
      } else if (budget.RemainingBudget < budget.TotalIncome * 0.1m) {
        output += "\n⚠️  ВНИМАНИЕ: Остаток бюджета критически мал!\n";
      } else if (budget.RemainingBudget > budget.TotalIncome * 0.3m) {
        output += "\n✅  ОТЛИЧНО: Вы откладываете более 30% дохода!\n";
      } else {
        output += "\n📌  Рекомендуется оптимизировать расходы.\n";
      }

      return output;
    }

    public string GetReportType() {
      return "CategoryReport";
    }
  }
}