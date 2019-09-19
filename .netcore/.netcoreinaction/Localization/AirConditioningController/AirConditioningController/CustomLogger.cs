using System;
using Microsoft.Extensions.Logging;

namespace AirConditioningController
{
    internal class CustomLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"{DateTime.Now} {logLevel} {formatter(state, exception)}");
        }
    }
}
