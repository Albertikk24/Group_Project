using BudgetApp.Models;

namespace BudgetApp.Factory {
  // Фабрика для категории "Коммунальные услуги"
  public class UtilitiesCategoryFactory : ICategoryFactory {
    private const decimal MONTHLY_LIMIT = 10000m;

    public Category CreateCategory() {
      return new Category(
        name: "Коммунальные услуги",
        description: "Квартплата, электричество, вода, газ",
        monthlyLimit: MONTHLY_LIMIT
      );
    }

    public string GetCategoryType() {
      return "Utilities";
    }
  }
}