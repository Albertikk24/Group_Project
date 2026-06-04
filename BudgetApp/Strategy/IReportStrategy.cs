using BudgetApp.BudgetApp.Models;

namespace BudgetApp.BudgetApp.Strategy {
  // Паттерн Strategy: интерфейс стратегии отчета
  public interface IReportStrategy {
    string GenerateReport(Budget budget, List<Expense> expenses);
    string GetReportType();
  }
}