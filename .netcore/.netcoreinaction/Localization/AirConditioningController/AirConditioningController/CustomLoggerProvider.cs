using Microsoft.Extensions.Logging;

namespace AirConditioningController
{
    internal class CustomLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new CustomLogger();

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}