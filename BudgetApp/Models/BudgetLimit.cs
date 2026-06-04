namespace BudgetApp.BudgetApp.Models {
  // Модель лимита бюджета по категории
  public class BudgetLimit {
    // ========== СВОЙСТВА ==========
    public string CategoryName { get; set; }
    public decimal MonthlyLimit { get; set; }
    public decimal CurrentSpending { get; set; }
    public DateTime Month { get; set; }

    // ========== КОНСТРУКТОР ==========
    public BudgetLimit(string categoryName, decimal monthlyLimit, DateTime month) {
      // Валидация входных данных
      if (string.IsNullOrWhiteSpace(categoryName)) {
        throw new ArgumentNullException(nameof(categoryName));
      }
      if (monthlyLimit <= 0) {
        throw new ArgumentException("Лимит должен быть положительным", nameof(monthlyLimit));
      }

      CategoryName = categoryName;
      MonthlyLimit = monthlyLimit;
      CurrentSpending = 0;
      Month = month;
    }

    // ========== БИЗНЕС-ЛОГИКА ==========
    // Оставшийся лимит
    public decimal GetRemainingLimit() {
      return MonthlyLimit - CurrentSpending;
    }

    // Процент использования лимита
    public decimal GetUsedPercent() {
      if (MonthlyLimit <= 0) {
        return 0;
      }
      return (CurrentSpending / MonthlyLimit) * 100;
    }

    // Проверка превышения
    public bool IsExceeded() {
      return CurrentSpending > MonthlyLimit;
    }

    // Уровень предупреждения (на основе процента)
    public string GetWarningLevel() {
      decimal percent = GetUsedPercent();

      if (percent >= 100) {
        return "КРИТИЧЕСКИЙ ПЕРЕРАСХОД";
      }
      if (percent >= 90) {
        return "ЛИМИТ НА ИСХОДЕ";
      }
      if (percent >= 75) {
        return "ВНИМАНИЕ";
      }
      if (percent >= 50) {
        return "НОРМАЛЬНО";
      }
      return "ХОРОШО";
    }

    // ========== ПЕРЕОПРЕДЕЛЕНИЕ ==========
    public override string ToString() {
      return $"{CategoryName}: {CurrentSpending:C} / {MonthlyLimit:C} ({GetUsedPercent():F0}%) - {GetWarningLevel()}";
    }
  }
}