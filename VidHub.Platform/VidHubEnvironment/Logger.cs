using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VidHub.Platform.VidHubEnvironment
{
    internal class LogEntry
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("level")]
        public string Level { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("exception")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Exception { get; set; } = null;
    }


    internal abstract class LoggerTemplate(LogLevel minLevel) : ILogger
    {
        private readonly object locker = new();

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= minLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            lock (locker)
            {
                LogAction(logLevel, eventId, state, exception, formatter);
            }
        }

        public abstract void LogAction<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);
    }

    internal class ConsoleLogger(LogLevel minLevel) : LoggerTemplate(minLevel)
    {
        public override void LogAction<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            string message = formatter(state, exception);
            string debugEntry = exception != null
                ? $"[{logLevel}] {message} ({exception.Message})"
                : $"[{logLevel}] {message}";

            Debug.WriteLine(debugEntry);
        }
    }

    internal class FileLogger : LoggerTemplate, IDisposable
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private readonly string logFilePath;
        private readonly Queue<string> logBuffer = new();
        private readonly object bufferLock = new();
        private Timer? flushTimer;

        private const int batchSize = 250;
        private const int flushIntervalMilliseconds = 5000;

        public FileLogger(LogLevel minLevel, string logFilePath) : base(minLevel)
        {
            this.logFilePath = logFilePath;

            string? directory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                _ = Directory.CreateDirectory(directory);
            }

            flushTimer = new Timer(_ => Flush(), null, flushIntervalMilliseconds, flushIntervalMilliseconds);
        }

        public override void LogAction<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = formatter(state, exception);
            LogEntry logEntry = new()
            {
                Timestamp = DateTime.UtcNow,
                Level = logLevel.ToString(),
                Message = message,
                Exception = exception?.ToString()
            };

            string json = JsonSerializer.Serialize(logEntry, JsonOptions);

            lock (bufferLock)
            {
                logBuffer.Enqueue(json);

                if (logBuffer.Count >= batchSize)
                {
                    Flush();
                }
            }
        }

        private void Flush()
        {
            lock (bufferLock)
            {
                if (logBuffer.Count == 0)
                {
                    return;
                }

                try
                {
                    using FileStream fileStream = new(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                    using StreamWriter writer = new(fileStream);
                    
                    while (logBuffer.TryDequeue(out string? json))
                    {
                        writer.WriteLine(json);
                    }
                }
                catch
                {
                }
            }
        }

        public void Dispose()
        {
            flushTimer?.Dispose();
            Flush();
            GC.SuppressFinalize(this);
        }
    }


    public class ConsoleLoggerProvider(LogLevel minLevel) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger(minLevel);
        }

        public void Dispose()
        {
        }
    }

    public class LastRunFileLoggerProvider(LogLevel minLevel) : ILoggerProvider
    {
        private readonly string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub", "vidhub.log");

        public ILogger CreateLogger(string categoryName)
        {
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
            return new FileLogger(minLevel, logFilePath);
        }
        public void Dispose()
        {
        }
    }

    public class PermanentFileLoggerProvider(LogLevel minLevel) : ILoggerProvider
    {
        private readonly string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub", "Logs", $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss}.log");

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(minLevel, logFilePath);
        }
        public void Dispose()
        {
        }
    }
}
