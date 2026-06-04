namespace BudgetApp.BudgetApp.Models {
  // Модель пользователя
  public class User {
    // ========== СВОЙСТВА ==========
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }

    // ========== КОНСТРУКТОР ==========
    public User(string username, string email, string passwordHash) {
      // Валидация входных данных
      if (string.IsNullOrWhiteSpace(username)) {
        throw new ArgumentNullException(nameof(username));
      }
      if (string.IsNullOrWhiteSpace(email)) {
        throw new ArgumentNullException(nameof(email));
      }
      if (string.IsNullOrWhiteSpace(passwordHash)) {
        throw new ArgumentNullException(nameof(passwordHash));
      }

      Username = username;
      Email = email;
      PasswordHash = passwordHash;
      CreatedAt = DateTime.Now;
    }

    // ========== ПЕРЕОПРЕДЕЛЕНИЕ ==========
    public override string ToString() {
      return $"{Username} ({Email}) - зарегистрирован {CreatedAt:dd.MM.yyyy}";
    }
  }
}