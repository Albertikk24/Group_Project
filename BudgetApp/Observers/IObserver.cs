namespace BudgetApp.BudgetApp.Observers {
  // Паттерн Наблюдатель: интерфейс наблюдателя
  public interface IObserver {
    void Update(string message, decimal currentBudget, decimal remaining);
    string GetObserverName();
  }
}