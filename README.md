# BudgetApp - Бюджетное приложение

## Описание проекта

Консольное приложение на C# для управления личным бюджетом. Программа позволяет создавать бюджет, отслеживать расходы, управлять сбережениями, устанавливать лимиты по категориям и генерировать отчеты.

## Участники проекта и их вклад

### Ветки и распределение ролей

| Ветка | Участник | Роль | Задачи |
|-------|----------|------|--------|
| `master` | ikkert | Team Lead | Стабильная версия, финальная интеграция |
| `dev` | Все | Разработка | Интеграционная ветка (НЕ ГОТОВЫЙ КОД) |
| `ikkert/tl` | ikkert | Team Lead | Стратегии отчетов, экспорт данных |
| `katkov` | katkov | Разработчик | Наблюдатель, бюджет, сбережения |
| `kondratenko` | kondratenko | Разработчик | Фабричный метод, категории, UI |

---

### Участник А (ikkert) - Team Lead

**Задачи в ветке `ikkert/tl`:**

| Файл | Задача | Паттерн |
|------|--------|---------|
| `Strategy/IReportStrategy.cs` | Интерфейс стратегии отчетов | Strategy |
| `Strategy/MonthlyReportStrategy.cs` | Месячный отчет | Strategy |
| `Strategy/YearlyReportStrategy.cs` | Годовой отчет | Strategy |
| `Strategy/CategoryReportStrategy.cs` | Отчет по категориям | Strategy |
| `Services/ExportService.cs` | Экспорт в CSV/JSON | - |
| `Infrastructure/Logger.cs` | Логирование | Singleton |
| `Program.cs` | Точка входа, инициализация | - |
| `Models/User.cs` | Модель пользователя | - |
| `BudgetApp.csproj` | Конфигурация проекта | - |
| `.gitignore` | Игнорирование файлов | - |
| `README.md` | Документация | - |

**Всего файлов: 11**

---

### Участник Б (katkov) - Разработчик

**Задачи в ветке `katkov`:**

| Файл | Задача | Паттерн |
|------|--------|---------|
| `Observers/IObserver.cs` | Интерфейс наблюдателя | Observer |
| `Observers/ISubject.cs` | Интерфейс субъекта | Observer |
| `Observers/UserNotifier.cs` | Уведомления пользователя | Observer |
| `Models/Budget.cs` | Модель бюджета | Observer |
| `Models/Expense.cs` | Модель расхода | - |
| `Models/Saving.cs` | Модель сбережения | - |
| `Models/BudgetLimit.cs` | Модель лимита | - |

**Всего файлов: 7**

---

### Участник В (kondratenko) - Разработчик

**Задачи в ветке `kondratenko`:**

| Файл | Задача | Паттерн |
|------|--------|---------|
| `Factory/ICategoryFactory.cs` | Интерфейс фабрики | Factory Method |
| `Factory/FoodCategoryFactory.cs` | Категория "Еда" | Factory Method |
| `Factory/TransportCategoryFactory.cs` | Категория "Транспорт" | Factory Method |
| `Factory/EntertainmentCategoryFactory.cs` | Категория "Развлечения" | Factory Method |
| `Factory/UtilitiesCategoryFactory.cs` | Категория "Коммунальные услуги" | Factory Method |
| `Factory/WorkCategoryFactory.cs` | Категория "Рабочие расходы" | Factory Method |
| `Factory/CategoryFactoryProvider.cs` | Провайдер фабрик | Factory Method |
| `Views/ConsoleView.cs` | Консольный интерфейс | - |
| `Controllers/BudgetController.cs` | Контроллер (интеграция) | - |

**Всего файлов: 9**

## Используемые паттерны

| Паттерн | Участник | Назначение |
|---------|----------|------------|
| **Strategy** | ikkert | Различные типы отчетов |
| **Singleton** | ikkert | Единый экземпляр логгера |
| **Observer** | katkov | Уведомления об изменениях бюджета |
| **Factory Method** | kondratenko | Создание категорий расходов |

---

## Функциональные возможности

- Создание бюджета с указанием дохода
- Добавление расходов по категориям (выбор по номеру)
- 5 категорий: Еда, Транспорт, Развлечения, Коммунальные услуги, Рабочие расходы
- Установка лимитов по категориям
- Автоматические уведомления о превышении лимита
- Управление целями сбережений (с датой или без)
- Генерация отчетов (месячный, годовой, по категориям)
- Экспорт данных в CSV и JSON
- Логирование всех действий
- Цветовой вывод в консоли (синий для меню, серый для логов)
