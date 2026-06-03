namespace BudgetApp.Models {
  public class Expense {
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string CategoryName { get; set; }

    public Expense(string description, decimal amount, string categoryName) {
      Description = description ?? throw new ArgumentNullException(nameof(description));
      Amount = amount;
      Date = DateTime.Now;
      CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
    }

    public override string ToString() {
      return $"{Date:dd.MM.yyyy} - {Description}: {Amount:C} ({CategoryName})";
    }
  }
}