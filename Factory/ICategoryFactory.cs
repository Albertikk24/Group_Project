using BudgetApp.Models;

namespace BudgetApp.Factory
{
  // Паттерн Factory Method: интерфейс фабрики
  public interface ICategoryFactory
  {
    Category CreateCategory();
    string GetCategoryType();
  }
}