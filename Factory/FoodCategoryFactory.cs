using BudgetApp.Models;

namespace BudgetApp.Factory {
  // Фабрика для категории "Еда"
  public class FoodCategoryFactory : ICategoryFactory {
    private const decimal MONTHLY_LIMIT = 15000m;

    public Category CreateCategory() {
      return new Category(
        name: "Еда",
        description: "Продукты питания, рестораны, кафе",
        monthlyLimit: MONTHLY_LIMIT
      );
    }

    public string GetCategoryType() {
      return "Food";
    }
  }
}