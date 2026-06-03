using BudgetApp.Models;

namespace BudgetApp.Factory
{
  public class FoodCategoryFactory : ICategoryFactory
  {
    public Category CreateCategory()
    {
      return new Category(
        name: "Еда",
        description: "Продукты питания, рестораны, кафе",
        monthlyLimit: 15000m
      );
    }

    public string GetCategoryType()
    {
      return "Food";
    }
  }
}