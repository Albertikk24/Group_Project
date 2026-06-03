using BudgetApp.Models;

namespace BudgetApp.Factory
{
  public class TransportCategoryFactory : ICategoryFactory
  {
    public Category CreateCategory()
    {
      return new Category(
        name: "Транспорт",
        description: "Бензин, такси, общественный транспорт",
        monthlyLimit: 5000m
      );
    }

    public string GetCategoryType()
    {
      return "Transport";
    }
  }
}