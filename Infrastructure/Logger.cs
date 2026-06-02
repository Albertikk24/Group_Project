namespace BudgetApp.Infrastructure {
  public sealed class Logger {
    private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
    public static Logger Instance => _instance.Value;

    private List<string> _logs;
    private readonly object _lock = new object();

    private Logger() {
      _logs = new List<string>();
      Log("Логгер инициализирован");
    }

    public void Log(string message) {
      string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      string logEntry = $"[{timestamp}] {message}";
      
      lock (_lock) {
        _logs.Add(logEntry);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(logEntry);
        Console.ResetColor();
      }
    }

    public void ShowAllLogs() {
      string output = "\n" + new string('=', 60) + "\n";
      output += "ИСТОРИЯ ЛОГОВ\n";
      output += new string('=', 60) + "\n";
      
      if (_logs.Count == 0) {
        output += "Логи отсутствуют\n";
      } else {
        foreach (var log in _logs) {
          output += log + "\n";
        }
      }
      
      output += new string('=', 60) + "\n";
      output += $"Всего записей: {_logs.Count}\n";
      
      Console.WriteLine(output);
    }
  }
}