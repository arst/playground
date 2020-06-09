using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace EventSourceConsumeProduce
{
    [EventSource(LocalizationResources = "EventSourceConsumeProduce.resources.EventSource")]
    public class SimpleEventSource : EventSource
    {
        [Event(1)]
        public void ReportOne(int report) => WriteEvent(1, report);

        [Event(2)]
        public void ReportTwo(string report) => WriteEvent(2, report);
    }
}
