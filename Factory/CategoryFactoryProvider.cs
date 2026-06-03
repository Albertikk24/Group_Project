using BudgetApp.Models;

namespace BudgetApp.Factory
{
  // Провайдер для работы со всеми фабриками
  public static class CategoryFactoryProvider
  {
    private static List<ICategoryFactory> _factories;

    static CategoryFactoryProvider()
    {
      _factories = new List<ICategoryFactory> {
        new FoodCategoryFactory(),
        new TransportCategoryFactory(),
        new EntertainmentCategoryFactory(),
        new UtilitiesCategoryFactory()
      };
    }

    public static List<Category> CreateAllCategories()
    {
      var categories = new List<Category>();
      foreach (var factory in _factories)
      {
        categories.Add(factory.CreateCategory());
      }
      return categories;
    }

    public static List<string> GetAllCategoryNames()
    {
      var names = new List<string>();
      foreach (var factory in _factories)
      {
        names.Add(factory.CreateCategory().Name);
      }
      return names;
    }

    public static Category? GetCategoryByName(string name)
    {
      foreach (var factory in _factories)
      {
        var category = factory.CreateCategory();
        if (category.Name == name)
        {
          return category;
        }
      }
      return null;
    }

    internal static IEnumerable<object> GetAllCategories()
    {
      throw new NotImplementedException();
    }
  }
}