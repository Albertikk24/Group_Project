using System;

namespace BudgetApp.Models {
  public class Category {
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal? MonthlyLimit { get; set; }

    public Category(string name, string description, decimal? monthlyLimit = null) {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Description = description ?? "";
      MonthlyLimit = monthlyLimit;
    }

    public bool IsLimitExceeded(decimal currentSpending) {
      return MonthlyLimit.HasValue && currentSpending > MonthlyLimit.Value;
    }

    public override string ToString() {
      string limitText = MonthlyLimit.HasValue ? $" (лимит: {MonthlyLimit:C})" : "";
      return $"{Name} - {Description}{limitText}";
    }
  }
}