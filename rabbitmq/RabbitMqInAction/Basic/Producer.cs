using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMqInAction.Basic
{
    internal sealed class Producer
    {
        internal void Produce(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            while (true)
            {
                try
                {
                    var messageContent = "Hello Rabbit!";

                    var messageBody = Encoding.UTF8.GetBytes(messageContent);
                
                    channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: messageBody);
                    System.Console.WriteLine("Produced");
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
