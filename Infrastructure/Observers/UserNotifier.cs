using System;
using BudgetApp.Infrastructure;

namespace groupProject.Infrastructure.Observers {
  // Конкретный наблюдатель
  public class UserNotifier : IObserver {
    private string _userName;
    private string _email;

    public UserNotifier(string userName, string email) {
      _userName = userName ?? throw new ArgumentNullException(nameof(userName));
      _email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public void Update(string message, decimal currentBudget, decimal remaining) {
      string notification = $@"
+---------------------------------------------------+
| УВЕДОМЛЕНИЕ ОБ ИЗМЕНЕНИИ БЮДЖЕТА                  |
+---------------------------------------------------+
| Пользователь: {_userName} <{_email}>
| Сообщение: {message}
| Текущий бюджет: {currentBudget:C}
| Остаток: {remaining:C}
+---------------------------------------------------+
|
| {(remaining < 0 ? "ВНИМАНИЕ! БЮДЖЕТ ПРЕВЫШЕН!" : 
    remaining < currentBudget * 0.1m ? "БЮДЖЕТ НА ИСХОДЕ!" : "Бюджет в порядке")}
+---------------------------------------------------+";
      
      Console.WriteLine(notification);
      Logger.Instance.Log($"Уведомление отправлено {_userName}: {message}");
    }

    public string GetObserverName() {
      return _userName;
    }
  }
}