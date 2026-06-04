namespace BudgetApp.BudgetApp.Observers {
  // Паттерн Наблюдатель: интерфейс наблюдателя за бюджетом
  public interface IBudgetObserver {
    // Получение уведомления об изменении бюджета
    // message - текст уведомления
    // currentBudget - текущий доход
    // remaining - остаток бюджета
    void Update(string message, decimal currentBudget, decimal remaining);
    
    // Получение имени наблюдателя
    string GetObserverName();
    
    // Получение роли наблюдателя (для логирования)
    string GetObserverRole();
  }
}