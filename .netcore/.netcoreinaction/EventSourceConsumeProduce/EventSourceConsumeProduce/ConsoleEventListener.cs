using System;
using System.Diagnostics.Tracing;

namespace EventSourceConsumeProduce
{
    public class ConsoleEventListener : EventListener
    {
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            base.OnEventSourceCreated(eventSource);
            Console.WriteLine("Event Source created");
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            Console.WriteLine(eventData.Message, eventData.Payload[0]);
        }
    }
}
