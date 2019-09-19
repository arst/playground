using System.Resources;
using Microsoft.Extensions.Logging;

namespace AirConditioningController
{
    internal class Telemetry
    {
        private ILogger<Telemetry> logger;

        public Telemetry(ILogger<Telemetry> logger)
        {
            this.logger = logger;
        }

        internal void LogStatus()
        {
            var resourceManager = new ResourceManager("AirConditioningController.messages", typeof(Program).Assembly);

            string message = $"{resourceManager.GetString("ExhaustAirTemp")}: {TemperatureController.EaxhaustAirTemperature}, {TemperatureController.OutsideAirTemperature}";
            logger.Log(LogLevel.Information, message);
        }

    }
}