using BudgetApp.Models;

namespace BudgetApp.Factories
{
  public static class CategoryFactoryProvider
  {
    private static List<Category> _categories;

    static CategoryFactoryProvider()
    {
      _categories = new List<Category>
            {
                new Category("Продукты", "Еда и напитки"),
                new Category("Транспорт", "Проезд, такси, бензин"),
                new Category("ЖКХ", "Коммунальные платежи"),
                new Category("Развлечения", "Кино, рестораны, игры"),
                new Category("Здоровье", "Лекарства, врачи, спорт"),
                new Category("Одежда", "Одежда и обувь"),
                new Category("Образование", "Курсы, книги"),
                new Category("Прочее", "Прочие расходы")
            };
    }

    // ВАЖНО: возвращаем List<Category>, а не List<object>!
    public static List<Category> GetAllCategories()
    {
      return new List<Category>(_categories);
    }

    public static Category GetCategoryByName(string name)
    {
      return _categories.Find(c => c.Name == name);
    }

    public static Category? GetCategoryById(int id)
    {
      if (id >= 0 && id < _categories.Count)
        return _categories[id];
      return null;
    }

    public static void AddCategory(string name, string description = "", decimal? monthlyLimit = null)
    {
      _categories.Add(new Category(name, description, monthlyLimit));
    }

    public static bool CategoryExists(string name)
    {
      return _categories.Exists(c => c.Name == name);
    }
  }
}