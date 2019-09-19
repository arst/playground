using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace AirConditioningController
{
    class Program
    {
        static void Main(string[] args)
        {
            var culture = new CultureInfo("es-MX");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            TemperatureController.Test();
        }
    }
}
