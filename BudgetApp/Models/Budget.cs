using BudgetApp.BudgetApp.Infrastructure;
using BudgetApp.BudgetApp.Observers;

namespace BudgetApp.BudgetApp.Models {
  // Модель бюджета с поддержкой паттерна Observer
  public class Budget : ISubject {
    // ========== ПОЛЯ ==========
    private List<IObserver> _observers = new List<IObserver>();

    // ========== СВОЙСТВА ==========
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal RemainingBudget { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Expense> Expenses { get; set; }

    // ========== КОНСТРУКТОР ==========
    public Budget(decimal totalIncome) {
      // Валидация входных данных
      if (totalIncome < 0) {
        throw new ArgumentException("Доход не может быть отрицательным", nameof(totalIncome));
      }

      TotalIncome = totalIncome;
      TotalExpenses = 0;
      RemainingBudget = totalIncome;
      CreatedAt = DateTime.Now;
      Expenses = new List<Expense>();
    }

    // ========== PATTERN OBSERVER ==========
    public void Attach(IObserver observer) {
      if (observer == null) {
        Logger.Instance.Log("Ошибка: наблюдатель не может быть null");
        return;
      }

      if (!_observers.Contains(observer)) {
        _observers.Add(observer);
        Logger.Instance.Log($"Подписан наблюдатель: {observer.GetObserverName()}");
      }
    }

    public void Detach(IObserver observer) {
      if (observer == null) {
        return;
      }

      _observers.Remove(observer);
      Logger.Instance.Log($"Отписан наблюдатель: {observer.GetObserverName()}");
    }

    // Оповещение всех подписанных наблюдателей
    public void Notify(string message, decimal currentBudget, decimal remaining) {
      for (int observerIndex = 0; observerIndex < _observers.Count; ++observerIndex) {
        IObserver currentObserver = _observers[observerIndex];
        currentObserver.Update(message, currentBudget, remaining);
      }
    }

    // ========== БИЗНЕС-ЛОГИКА ==========
    public void AddExpense(Expense expense) {
      if (expense == null) {
        Logger.Instance.Log("Ошибка: расход не может быть null");
        return;
      }

      Expenses.Add(expense);
      TotalExpenses += expense.Amount;
      UpdateRemainingBudget();
      // Уведомление наблюдателей о новом расходе
      Notify("Добавлен новый расход", TotalIncome, RemainingBudget);
    }

    public void UpdateRemainingBudget() {
      RemainingBudget = TotalIncome - TotalExpenses;
    }

    // ========== ПЕРЕОПРЕДЕЛЕНИЕ ==========
    public override string ToString() {
      return $"Бюджет: доход {TotalIncome:C}, расходы {TotalExpenses:C}, остаток {RemainingBudget:C}";
    }
  }
}