namespace BudgetApp.Models
{
  // Модель цели сбережения
  public class Saving
  {
    public string Name { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? TargetDate { get; set; }
    public bool IsCompleted { get; set; }

    public Saving(string name, decimal targetAmount, DateTime? targetDate = null)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      TargetAmount = targetAmount;
      CurrentAmount = 0;
      CreatedAt = DateTime.Now;
      TargetDate = targetDate;
      IsCompleted = false;
    }

    public void AddMoney(decimal amount)
    {
      CurrentAmount += amount;
      if (CurrentAmount >= TargetAmount)
      {
        IsCompleted = true;
        CurrentAmount = TargetAmount;
      }
    }

    public decimal GetProgressPercent()
    {
      if (TargetAmount <= 0) return 0;
      return (CurrentAmount / TargetAmount) * 100;
    }

    public int GetDaysRemaining()
    {
      if (!TargetDate.HasValue) return -1;
      int days = (TargetDate.Value - DateTime.Now).Days;
      return days > 0 ? days : 0;
    }

    public decimal GetMonthlyRecommendation()
    {
      if (!TargetDate.HasValue) return 0;
      int monthsRemaining = GetDaysRemaining() / 30;
      if (monthsRemaining <= 0) return 0;
      decimal remainingAmount = TargetAmount - CurrentAmount;
      return remainingAmount / monthsRemaining;
    }

    public override string ToString()
    {
      string targetDateText = TargetDate.HasValue ? $", до {TargetDate.Value:dd.MM.yyyy} ({GetDaysRemaining()} дн.)" : "";
      string completedText = IsCompleted ? " [ВЫПОЛНЕНО!]" : "";
      return $"{Name}: {CurrentAmount:C} из {TargetAmount:C} ({GetProgressPercent():F0}%){targetDateText}{completedText}";
    }
  }
}