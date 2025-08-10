using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.customLoggers
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _fileName;
        public FileLoggerProvider(string fileName)
        {
            _fileName = fileName;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_fileName);
        }

        public void Dispose()
        {}
    }
}
