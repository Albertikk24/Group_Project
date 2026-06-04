using System;
using System.Collections.Generic;

namespace BudgetApp.BudgetApp.Infrastructure {
  // Паттерн Одиночка (Singleton) - потокобезопасный логгер
  public sealed class Logger {
    // ========== ОДИНОЧКА ==========
    private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
    public static Logger Instance => _instance.Value;

    // ========== ПОЛЯ ==========
    private List<string> _logs;
    private readonly object _lock = new object();
    private const int MAX_LOG_ENTRIES = 1000;  // Ограничение размера лога

    // ========== ПРИВАТНЫЙ КОНСТРУКТОР ==========
    private Logger() {
      _logs = new List<string>();
      Log("Логгер инициализирован");
    }

    // ========== ДОБАВЛЕНИЕ ЗАПИСИ В ЛОГ ==========
    public void Log(string message) {
      string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      string logEntry = $"[{timestamp}] {message}";

      lock (_lock) {
        _logs.Add(logEntry);
        // Ограничение размера лога (удаляем старые записи)
        if (_logs.Count > MAX_LOG_ENTRIES) {
          _logs.RemoveAt(0);
        }
        // Вывод в консоль серым цветом
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(logEntry);
        Console.ResetColor();
      }
    }

    // ========== ВЫВОД ВСЕХ ЛОГОВ ==========
    public void ShowAllLogs() {
      // Формирование одного вывода
      string output = "\n" + new string('=', 60) + "\n";
      output += "ИСТОРИЯ ЛОГОВ\n";
      output += new string('=', 60) + "\n";

      if (_logs.Count == 0) {
        output += "Логи отсутствуют\n";
      } else {
        for (int logIndex = 0; logIndex < _logs.Count; ++logIndex) {
          output += _logs[logIndex] + "\n";
        }
      }

      output += new string('=', 60) + "\n";
      output += $"Всего записей: {_logs.Count}\n";
      Console.WriteLine(output);
    }
  }
}