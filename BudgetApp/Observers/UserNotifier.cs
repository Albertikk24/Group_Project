using BudgetApp.BudgetApp.Infrastructure;

namespace BudgetApp.BudgetApp.Observers {
  // Конкретный наблюдатель - уведомляет пользователя
  public class UserNotifier : IObserver {
    // ========== ПОЛЯ ==========
    private string _userName;
    private string _email;

    // ========== КОНСТРУКТОР ==========
    public UserNotifier(string userName, string email) {
      if (string.IsNullOrWhiteSpace(userName)) {
        throw new ArgumentNullException(nameof(userName));
      }
      if (string.IsNullOrWhiteSpace(email)) {
        throw new ArgumentNullException(nameof(email));
      }

      _userName = userName;
      _email = email;
    }

    // ========== ПОЛУЧЕНИЕ УВЕДОМЛЕНИЯ ==========
    public void Update(string message, decimal currentBudget, decimal remaining) {
      // Определение уровня тревоги
      string warningLevel = "";
      if (remaining < 0) {
        warningLevel = "ВНИМАНИЕ! БЮДЖЕТ ПРЕВЫШЕН!";
      } else if (remaining < currentBudget * 0.1m) {
        warningLevel = "БЮДЖЕТ НА ИСХОДЕ!";
      } else {
        warningLevel = "Бюджет в порядке";
      }

      // Формирование уведомления в виде таблицы
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
| {warningLevel}
+---------------------------------------------------+";

      Console.WriteLine(notification);
      Logger.Instance.Log($"Уведомление отправлено {_userName}: {message}");
    }

    // ========== ПОЛУЧЕНИЕ ДАННЫХ ==========
    public string GetObserverName() {
      return _userName;
    }
  }
}