using BudgetApp.Models;

namespace BudgetApp.Factory
{
  public class EntertainmentCategoryFactory : ICategoryFactory
  {
    public Category CreateCategory()
    {
      return new Category(
        name: "Развлечения",
        description: "Кино, игры, хобби, отдых",
        monthlyLimit: 8000m
      );
    }

    public string GetCategoryType()
    {
      return "Entertainment";
    }
  }
}