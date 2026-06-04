using BudgetApp.BudgetApp.Models;

namespace BudgetApp.BudgetApp.Factory {
  // Паттерн Factory Method: интерфейс фабрики категорий
  public interface ICategoryFactory {
    Category CreateCategory();
    string GetCategoryType();
  }
}