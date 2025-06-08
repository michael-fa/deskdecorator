using System;
using System.Diagnostics;
using System.IO;
using System.Text;

public class Logger
{
    private readonly string _logFilePath;
    private static readonly object _lock = new();

    // Singleton instance for global access
    public static Logger Instance { get; private set; }

    static Logger()
    {
        Instance = new Logger();
    }

    private Logger()
    {
        // Ensure the logs directory exists
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        Directory.CreateDirectory(logDirectory);

        // Create a new log file with the startup time and date in its name
        string fileName = $"log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
        _logFilePath = Path.Combine(logDirectory, fileName);

        // Write initial message to the log file
        WriteLine("=== Application Started ===");

        // Add a custom trace listener to capture Debug/Trace output
        Trace.Listeners.Add(new LogTraceListener(this));
    }

    /// <summary>
    /// Logs a message to the console and the log file.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void Log(string message)
    {
        string timestampedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        Console.WriteLine(timestampedMessage);
        AppendToFile(timestampedMessage);
    }

    /// <summary>
    /// Logs a message to the console and the log file, formatted with additional arguments.
    /// </summary>
    /// <param name="message">The message template.</param>
    /// <param name="args">Arguments to format into the message.</param>
    public void Log(string message, params object[] args)
    {
        Log(string.Format(message, args));
    }

    /// <summary>
    /// Writes a line to the log file only.
    /// </summary>
    /// <param name="message">The message to write.</param>
    public void WriteLine(string message)
    {
        string timestampedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        AppendToFile(timestampedMessage);
    }

    /// <summary>
    /// Logs an exception in a human-readable format.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    public void LogException(Exception ex)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== Exception Caught ===");
        sb.AppendLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"Message: {ex.Message}");
        sb.AppendLine("Stack Trace:");
        sb.AppendLine(ex.StackTrace);

        if (ex.InnerException != null)
        {
            sb.AppendLine("Inner Exception:");
            AppendInnerExceptionDetails(sb, ex.InnerException);
        }

        sb.AppendLine("========================");

        // Log the exception details
        Log(sb.ToString());
    }

    private void AppendInnerExceptionDetails(StringBuilder sb, Exception ex)
    {
        sb.AppendLine($"Message: {ex.Message}");
        sb.AppendLine("Stack Trace:");
        sb.AppendLine(ex.StackTrace);

        if (ex.InnerException != null)
        {
            sb.AppendLine("Further Inner Exception:");
            AppendInnerExceptionDetails(sb, ex.InnerException);
        }
    }

    private void AppendToFile(string message)
    {
        lock (_lock)
        {
            File.AppendAllText(_logFilePath, message + Environment.NewLine);
        }
    }

    // Custom TraceListener to redirect Debug and Trace calls to the LogSystem
    private class LogTraceListener : TraceListener
    {
        private readonly Logger _logSystem;

        public LogTraceListener(Logger logSystem)
        {
            _logSystem = logSystem;
        }

        public override void Write(string message)
        {
            _logSystem.Log(message);
        }

        public override void WriteLine(string message)
        {
            _logSystem.Log(message);
        }
    }
}
