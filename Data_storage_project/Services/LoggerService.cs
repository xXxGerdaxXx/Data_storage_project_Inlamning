using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Services;

public class LoggerService : ILoggerService
{
    public void LogError(string message, Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR]: {message} - {ex}");
        Console.ResetColor();
    }

    public void LogWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARNING]: {message}");
        Console.ResetColor();
    }

    public void LogInformation(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[INFO]: {message}");
        Console.ResetColor();
    }
}
