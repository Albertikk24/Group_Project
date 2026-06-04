using BudgetApp.BudgetApp.Models;
using BudgetApp.Factory;

namespace BudgetApp.BudgetApp.Factory {
  // Провайдер для работы со всеми фабриками
  public static class CategoryFactoryProvider {
    private static List<ICategoryFactory> _factories;

    // Статический конструктор - инициализация фабрик
    static CategoryFactoryProvider() {
      _factories = new List<ICategoryFactory> {
        new FoodCategoryFactory(),
        new TransportCategoryFactory(),
        new EntertainmentCategoryFactory(),
        new UtilitiesCategoryFactory(),
        new WorkCategoryFactory()      // НОВАЯ КАТЕГОРИЯ
      };
    }

    // Создание всех категорий через фабрики
    public static List<Category> CreateAllCategories() {
      List<Category> categories = new List<Category>();
      for (int factoryIndex = 0; factoryIndex < _factories.Count; ++factoryIndex) {
        ICategoryFactory currentFactory = _factories[factoryIndex];
        categories.Add(currentFactory.CreateCategory());
      }
      return categories;
    }

    // Получение названий всех категорий
    public static List<string> GetAllCategoryNames() {
      List<string> names = new List<string>();
      for (int factoryIndex = 0; factoryIndex < _factories.Count; ++factoryIndex) {
        ICategoryFactory currentFactory = _factories[factoryIndex];
        names.Add(currentFactory.CreateCategory().Name);
      }
      return names;
    }

    // Поиск категории по названию
    public static Category? GetCategoryByName(string name) {
      for (int factoryIndex = 0; factoryIndex < _factories.Count; ++factoryIndex) {
        ICategoryFactory currentFactory = _factories[factoryIndex];
        Category category = currentFactory.CreateCategory();
        if (category.Name == name) {
          return category;
        }
      }
      return null;
    }
  }
}