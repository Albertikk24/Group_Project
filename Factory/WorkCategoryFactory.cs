using BudgetApp.Models;

namespace BudgetApp.Factory {
  // Фабрика для категории "Рабочие расходы"
  public class WorkCategoryFactory : ICategoryFactory {
    private const decimal MONTHLY_LIMIT = 5000m;

    public Category CreateCategory() {
      return new Category(
        name: "Рабочие расходы",
        description: "Канцелярия, обучение, профессиональные услуги",
        monthlyLimit: MONTHLY_LIMIT
      );
    }

    public string GetCategoryType() {
      return "Work";
    }
  }
}