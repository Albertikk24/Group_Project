using BudgetApp.Models;

namespace BudgetApp.Factory {
  // Паттерн Factory Method: интерфейс фабрики категорий
  public interface ICategoryFactory {
    Category CreateCategory();
    string GetCategoryType();
  }
}