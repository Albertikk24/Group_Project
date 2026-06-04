using BudgetApp.BudgetApp.Models;

namespace BudgetApp.BudgetApp.Factory {
  // Фабрика для категории "Транспорт"
  public class TransportCategoryFactory : ICategoryFactory {
    private const decimal MONTHLY_LIMIT = 5000m;

    public Category CreateCategory() {
      return new Category(
        name: "Транспорт",
        description: "Бензин, такси, общественный транспорт",
        monthlyLimit: MONTHLY_LIMIT
      );
    }

    public string GetCategoryType() {
      return "Transport";
    }
  }
}