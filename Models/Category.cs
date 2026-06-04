namespace BudgetApp.Models {
  // Модель категории расходов
  public class Category {
    // ========== СВОЙСТВА ==========
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal? MonthlyLimit { get; set; }

    // ========== КОНСТРУКТОР ==========
    public Category(string name, string description, decimal? monthlyLimit = null) {
      // Валидация входных данных
      if (string.IsNullOrWhiteSpace(name)) {
        throw new ArgumentNullException(nameof(name));
      }

      Name = name;
      Description = description ?? "";
      MonthlyLimit = monthlyLimit;
    }

    // Проверка превышения лимита
    public bool IsLimitExceeded(decimal currentSpending) {
      return MonthlyLimit.HasValue && currentSpending > MonthlyLimit.Value;
    }

    // ========== ПЕРЕОПРЕДЕЛЕНИЕ ==========
    public override string ToString() {
      string limitText = MonthlyLimit.HasValue ? $" (лимит: {MonthlyLimit:C})" : "";
      return $"{Name} - {Description}{limitText}";
    }
  }
}