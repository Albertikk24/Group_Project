using BudgetApp.BudgetApp.Models;

namespace BudgetApp.BudgetApp.Factory {
  // Фабрика для категории "Развлечения"
  public class EntertainmentCategoryFactory : ICategoryFactory {
    private const decimal MONTHLY_LIMIT = 8000m;

    public Category CreateCategory() {
      return new Category(
        name: "Развлечения",
        description: "Кино, игры, хобби, отдых",
        monthlyLimit: MONTHLY_LIMIT
      );
    }

    public string GetCategoryType() {
      return "Entertainment";
    }
  }
}