using Microsoft.Extensions.Logging;

namespace AirConditioningController
{
    public class TemperatureController
    {
        private static readonly LoggerFactory loggerFactory;
        private static readonly Telemetry telemetry;

        public static double EaxhaustAirTemperature { get; set; }

        public static double CoolantTemperature { get; set; }

        public static double OutsideAirTemperature { get; set; }

        static TemperatureController()
        {
            loggerFactory = new LoggerFactory()
                .AddCustomLogger();
            telemetry = new Telemetry(loggerFactory.CreateLogger<Telemetry>());
        }

        public static void Test()
        {
            telemetry.LogStatus();
        }
    }
}
