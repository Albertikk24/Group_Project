namespace BudgetApp.BudgetApp.Models {
  // Модель цели сбережения
  public class Saving {
    // ========== СВОЙСТВА ==========
    public string Name { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? TargetDate { get; set; }
    public bool IsCompleted { get; set; }

    // ========== КОНСТРУКТОР ==========
    public Saving(string name, decimal targetAmount, DateTime? targetDate = null) {
      // Валидация входных данных
      if (string.IsNullOrWhiteSpace(name)) {
        throw new ArgumentNullException(nameof(name));
      }
      if (targetAmount <= 0) {
        throw new ArgumentException("Целевая сумма должна быть положительной", nameof(targetAmount));
      }

      Name = name;
      TargetAmount = targetAmount;
      CurrentAmount = 0;
      CreatedAt = DateTime.Now;
      TargetDate = targetDate;
      IsCompleted = false;
    }

    // ========== БИЗНЕС-ЛОГИКА ==========
    public void AddMoney(decimal amount) {
      if (amount <= 0) {
        return;
      }

      CurrentAmount += amount;
      // Проверка достижения цели
      if (CurrentAmount >= TargetAmount) {
        IsCompleted = true;
        CurrentAmount = TargetAmount;
      }
    }

    // Расчет процента выполнения
    public decimal GetProgressPercent() {
      if (TargetAmount <= 0) {
        return 0;
      }
      return (CurrentAmount / TargetAmount) * 100;
    }

    // Расчет оставшихся дней
    public int GetDaysRemaining() {
      if (!TargetDate.HasValue) {
        return -1;
      }

      int days = (TargetDate.Value - DateTime.Now).Days;
      return days > 0 ? days : 0;
    }

    // Расчет рекомендуемого ежемесячного взноса
    public decimal GetMonthlyRecommendation() {
      if (!TargetDate.HasValue) {
        return 0;
      }

      int monthsRemaining = GetDaysRemaining() / 30;
      if (monthsRemaining <= 0) {
        return 0;
      }

      decimal remainingAmount = TargetAmount - CurrentAmount;
      return remainingAmount / monthsRemaining;
    }

    // ========== ПЕРЕОПРЕДЕЛЕНИЕ ==========
    public override string ToString() {
      string targetDateText = "";
      if (TargetDate.HasValue) {
        targetDateText = $", до {TargetDate.Value:dd.MM.yyyy} ({GetDaysRemaining()} дн.)";
      }

      string completedText = IsCompleted ? " [ВЫПОЛНЕНО!]" : "";
      return $"{Name}: {CurrentAmount:C} из {TargetAmount:C} ({GetProgressPercent():F0}%){targetDateText}{completedText}";
    }
  }
}