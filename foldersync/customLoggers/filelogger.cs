using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.customLoggers
{
    public class FileLogger : ILogger
    {
        private string _fileName;

        public FileLogger(string fileName)
        {
            _fileName = fileName;
            using (var stream = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, 0));
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            using (var writeStream = new StreamWriter(_fileName, true))
            {
                writeStream.WriteLine(formatter(state, exception));
            }
            
        }

        public void LogInformation(string? message, params object?[] args)
        {
            Log<string>(LogLevel.Information, default, null, null, (state, ex) => string.Format(state, args));
        }
    }
}
