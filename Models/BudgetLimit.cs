namespace BudgetApp.Models {
  public class BudgetLimit {
    public string CategoryName { get; set; }
    public decimal MonthlyLimit { get; set; }
    public decimal CurrentSpending { get; set; }
    public DateTime Month { get; set; }

    public BudgetLimit(string categoryName, decimal monthlyLimit, DateTime month) {
      CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
      MonthlyLimit = monthlyLimit;
      CurrentSpending = 0;
      Month = month;
    }

    public decimal GetRemainingLimit() {
      return MonthlyLimit - CurrentSpending;
    }

    public decimal GetUsedPercent() {
      if (MonthlyLimit <= 0) return 0;
      return (CurrentSpending / MonthlyLimit) * 100;
    }

    public bool IsExceeded() {
      return CurrentSpending > MonthlyLimit;
    }

    public string GetWarningLevel() {
      decimal percent = GetUsedPercent();
      if (percent >= 100) return "КРИТИЧЕСКИЙ ПЕРЕРАСХОД";
      if (percent >= 90) return "ЛИМИТ НА ИСХОДЕ";
      if (percent >= 75) return "ВНИМАНИЕ";
      if (percent >= 50) return "НОРМАЛЬНО";
      return "ХОРОШО";
    }

    public override string ToString() {
      return $"{CategoryName}: {CurrentSpending:C} / {MonthlyLimit:C} ({GetUsedPercent():F0}%) - {GetWarningLevel()}";
    }

    internal static object Find(Func<object, bool> value)
    {
      throw new NotImplementedException();
    }
  }
}