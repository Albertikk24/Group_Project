using BudgetApp.Infrastructure;
using BudgetApp.Models;
using groupProject.Observers;

namespace groupProject.Models {
  public class Budget : ISubject {
    private List<IObserver> _observers = new List<IObserver>();
    
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal RemainingBudget { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Expense> Expenses { get; set; }

    public Budget(decimal totalIncome) {
      TotalIncome = totalIncome;
      TotalExpenses = 0;
      RemainingBudget = totalIncome;
      CreatedAt = DateTime.Now;
      Expenses = new List<Expense>();
    }

    public void Attach(IObserver observer) {
      if (!_observers.Contains(observer)) {
        _observers.Add(observer);
        Logger.Instance.Log($"Подписан наблюдатель: {observer.GetObserverName()}");
      }
    }

    public void Detach(IObserver observer) {
      _observers.Remove(observer);
      Logger.Instance.Log($"Отписан наблюдатель: {observer.GetObserverName()}");
    }

    public void Notify(string message, decimal currentBudget, decimal remaining) {
      foreach (var observer in _observers) {
        observer.Update(message, currentBudget, remaining);
      }
    }

    public void AddExpense(Expense expense) {
      Expenses.Add(expense);
      TotalExpenses += expense.Amount;
      UpdateRemainingBudget();
      Notify("Добавлен новый расход", TotalIncome, RemainingBudget);
    }

    public void UpdateRemainingBudget() {
      RemainingBudget = TotalIncome - TotalExpenses;
    }

    public override string ToString() {
      return $"Бюджет: доход {TotalIncome:C}, расходы {TotalExpenses:C}, остаток {RemainingBudget:C}";
    }
  }
}