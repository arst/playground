using RabbitMqInAction.Basic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMqInAction
{
    class Program
    {
        static void Main(string[] args)
        {
            var consumer = new Consumer();
            var producer = new Producer();
            var cancellationTokenSource = new CancellationTokenSource();

            _ = Task.Run(() => producer.Produce(cancellationTokenSource.Token));
            _ = Task.Run(() => consumer.Consume(cancellationTokenSource.Token));

            Console.ReadLine();
            cancellationTokenSource.Cancel();
        }
    }
}
