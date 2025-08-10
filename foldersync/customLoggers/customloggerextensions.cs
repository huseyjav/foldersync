using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.customLoggers
{
    public static class CustomLoggerExtensions
    {
        public static ILoggingBuilder AddCustomConsoleLogger(this ILoggingBuilder builder)
        {
            builder.AddProvider(new ConsoleLoggerProvider());   
            return builder;
        }
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, string fileName)
        {
            builder.AddProvider(new FileLoggerProvider(fileName));
            return builder;
        }
    }
}
