using BudgetApp.Models;

namespace BudgetApp.Factory
{
  public class UtilitiesCategoryFactory : ICategoryFactory
  {
    public Category CreateCategory()
    {
      return new Category(
        name: "Коммунальные услуги",
        description: "Квартплата, электричество, вода, газ",
        monthlyLimit: 10000m
      );
    }

    public string GetCategoryType()
    {
      return "Utilities";
    }
  }
}