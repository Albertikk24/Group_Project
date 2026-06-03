namespace groupProject.Infrastructure.Observers {
  // Паттерн Observer: интерфейс наблюдаемого объекта
  public interface ISubject {
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string message, decimal currentBudget, decimal remaining);
  }
}