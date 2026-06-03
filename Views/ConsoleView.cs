menu += "\n 7. Управление лимитами";

case "7": ManageLimits(); break;

private void ManageLimits() {
  Console.Clear();
  string limitsMenu = "\n=== УПРАВЛЕНИЕ ЛИМИТАМИ ===\n";
  limitsMenu += " 1. Показать все лимиты\n";
  limitsMenu += " 2. Установить лимит категории\n";
  limitsMenu += " 3. Проверить лимит категории\n";
  limitsMenu += " 4. Проверить все лимиты\n";
  limitsMenu += " 0. Назад\n";
  limitsMenu += "Выберите действие: ";

  Console.Write(limitsMenu);
  string choice = Console.ReadLine();

  switch (choice) {
    case "1":
      _controller.ShowAllLimits();
      WaitForUser();
      break;
    case "2":
      _controller.ShowCategories();
      Console.Write("\nНазвание категории: ");
      string? category = Console.ReadLine();
      Console.Write("Месячный лимит: ");
      if (decimal.TryParse(Console.ReadLine(), out decimal limit)) {
        _controller.SetCategoryLimit(category, limit);
      }
      WaitForUser();
      break;
    case "3":
      _controller.ShowCategories();
      Console.Write("\nНазвание категории: ");
      string? catName = Console.ReadLine();
      _controller.ShowLimitByCategory(catName);
      WaitForUser();
      break;
    case "4":
      _controller.CheckAllLimits();
      WaitForUser();
      break;
  }
}