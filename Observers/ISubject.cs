namespace BudgetApp.Observers {
  // Паттерн Наблюдатель: интерфейс наблюдаемого объекта
  public interface ISubject {
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string message, decimal currentBudget, decimal remaining);
  }
}