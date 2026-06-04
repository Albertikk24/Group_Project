namespace BudgetApp.Models {
  // Модель расхода
  public class Expense {
    // ========== СВОЙСТВА ==========
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string CategoryName { get; set; }

    // ========== КОНСТРУКТОР ==========
    public Expense(string description, decimal amount, string categoryName) {
      // Валидация входных данных
      if (string.IsNullOrWhiteSpace(description)) {
        throw new ArgumentNullException(nameof(description));
      }
      if (amount <= 0) {
        throw new ArgumentException("Сумма расхода должна быть положительной", nameof(amount));
      }
      if (string.IsNullOrWhiteSpace(categoryName)) {
        throw new ArgumentNullException(nameof(categoryName));
      }

      Description = description;
      Amount = amount;
      Date = DateTime.Now;
      CategoryName = categoryName;
    }

    // ========== ПЕРЕОПРЕДЕЛЕНИЕ ==========
    public override string ToString() {
      return $"{Date:dd.MM.yyyy} - {Description}: {Amount:C} ({CategoryName})";
    }
  }
}