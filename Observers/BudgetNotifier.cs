using BudgetApp.Infrastructure;

namespace BudgetApp.Observers {
  // Конкретный наблюдатель: уведомляет пользователя об изменениях бюджета
  public class BudgetNotifier : IBudgetObserver {
    private string _userName;
    private string _email;

    // ========== КОНСТРУКТОР ==========
    public BudgetNotifier(string userName, string email) {
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
      string warningLevel = GetWarningLevel(remaining, currentBudget);

      // Формирование уведомления в виде таблицы (один вывод)
      string notification = $@"
+---------------------------------------------------+
| УВЕДОМЛЕНИЕ ОБ ИЗМЕНЕНИИ БЮДЖЕТА                  |
+---------------------------------------------------+
| Пользователь: {_userName} <{_email}>
| Сообщение: {message}
| Текущий бюджет: {currentBudget:C}
| Остаток: {remaining:C}
+---------------------------------------------------+
| Статус: {warningLevel}
+---------------------------------------------------+";

      Console.WriteLine(notification);
      Logger.Instance.Log($"Уведомление отправлено {_userName}: {message}");
    }

    // Определение уровня тревоги на основе остатка
    private string GetWarningLevel(decimal remaining, decimal currentBudget) {
      if (remaining < 0) {
        return "КРИТИЧЕСКИЙ ПЕРЕРАСХОД!";
      }
      if (remaining < currentBudget * 0.1m) {
        return "БЮДЖЕТ НА ИСХОДЕ!";
      }
      if (remaining < currentBudget * 0.25m) {
        return "ОСТАТОК МАЛ";
      }
      if (remaining > currentBudget * 0.5m) {
        return "ОТЛИЧНАЯ ЭКОНОМИЯ";
      }
      return "Бюджет в порядке";
    }

    // ========== ПОЛУЧЕНИЕ ДАННЫХ ==========
    public string GetObserverName() {
      return _userName;
    }

    public string GetObserverRole() {
      return "Пользователь";
    }
  }
}