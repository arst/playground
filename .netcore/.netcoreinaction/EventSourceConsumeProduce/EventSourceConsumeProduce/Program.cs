using System;
using System.Diagnostics.Tracing;

namespace EventSourceConsumeProduce
{
    class Program
    {
        private static readonly SimpleEventSource Events = new SimpleEventSource();

        static void Main(string[] args)
        {
            using (var listener = new ConsoleEventListener())
            {
                listener.EnableEvents(Events, EventLevel.Verbose);
                Report();
            }
        }

        static void Report()
        {
            Events.ReportOne(100);
            Events.ReportTwo("This is an event");
        }
    }
}
