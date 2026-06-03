using BudgetApp.Infrastructure;
using BudgetApp.Models;

private List<BudgetLimit> _budgetLimits;

// В конструктор добавить:
_budgetLimits = new List<BudgetLimit>();

// ========== УПРАВЛЕНИЕ ЛИМИТАМИ ==========
public void SetCategoryLimit(string categoryName, decimal monthlyLimit)
{
  DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

  var existingLimit = _budgetLimits.Find(l =>
    l.CategoryName == categoryName && l.Month == currentMonth);

  if (existingLimit != null)
  {
    existingLimit.MonthlyLimit = monthlyLimit;
    Logger.Instance.Log($"Обновлен лимит для категории {categoryName}: {monthlyLimit:C}");
  }
  else
  {
    BudgetLimit newLimit = new BudgetLimit(categoryName, monthlyLimit, currentMonth);
    _budgetLimits.Add(newLimit);
    Logger.Instance.Log($"Установлен лимит для категории {categoryName}: {monthlyLimit:C}");
  }
}

public void UpdateLimitSpending()
{
  DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

  foreach (var limit in _budgetLimits)
  {
    if (limit.Month == currentMonth)
    {
      decimal spending = GetCurrentMonthSpendingByCategory(limit.CategoryName);
      limit.CurrentSpending = spending;
    }
  }
}

public void ShowAllLimits()
{
  UpdateLimitSpending();

  string output = "\n=== ЛИМИТЫ КАТЕГОРИЙ ===\n";
  if (_budgetLimits.Count == 0)
  {
    output += "Лимиты не установлены\n";
  }
  else
  {
    foreach (var limit in _budgetLimits)
    {
      output += $"  {limit}\n";
    }
  }
  Console.WriteLine(output);
}

public void ShowLimitByCategory(string categoryName)
{
  UpdateLimitSpending();
  DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

  var limit = _budgetLimits.Find(l =>
    l.CategoryName == categoryName && l.Month == currentMonth);

  if (limit == null)
  {
    Console.WriteLine($"Лимит для категории '{categoryName}' не установлен");
  }
  else
  {
    string output = $"\n=== ЛИМИТ: {categoryName} ===\n";
    output += $"Лимит: {limit.MonthlyLimit:C}\n";
    output += $"Потрачено: {limit.CurrentSpending:C}\n";
    output += $"Осталось: {limit.GetRemainingLimit():C}\n";
    output += $"Статус: {limit.GetWarningLevel()}\n";
    Console.WriteLine(output);
  }
}

public void CheckAllLimits()
{
  UpdateLimitSpending();
  DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

  foreach (var limit in _budgetLimits)
  {
    if (limit.Month == currentMonth && limit.IsExceeded())
    {
      Console.WriteLine($"\nПРЕВЫШЕН ЛИМИТ ПО КАТЕГОРИИ '{limit.CategoryName}'!");
      Console.WriteLine($"Лимит: {limit.MonthlyLimit:C}, потрачено: {limit.CurrentSpending:C}, перерасход: {limit.CurrentSpending - limit.MonthlyLimit:C}\n");
      Notify($"Превышен лимит по категории {limit.CategoryName}",
             _currentBudget.TotalIncome, _currentBudget.RemainingBudget);
    }
  }
}

// В методе AddExpense добавить вызов:
CheckAllLimits();