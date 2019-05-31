﻿using System;
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

            var resourceManager = new ResourceManager("AirConditioningController.messages", typeof(Program).Assembly);

            Console.WriteLine($"{resourceManager.GetString("ExhaustAirTemp")}: {TemperatureController.EaxhaustAirTemperature} ");
        }
    }
}
