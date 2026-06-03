namespace groupProject.Observers {
  // Паттерн Observer: интерфейс наблюдателя
  public interface IObserver {
    void Update(string message, decimal currentBudget, decimal remaining);
    string GetObserverName();
  }
}