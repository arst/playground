using Microsoft.Extensions.Logging;

namespace AirConditioningController
{
    public static class LoggerFactoryExtensions
    {
        public static LoggerFactory AddCustomLogger(this LoggerFactory factory)
        {
            factory.AddProvider(new CustomLoggerProvider());

            return factory;
        }
    }
}
