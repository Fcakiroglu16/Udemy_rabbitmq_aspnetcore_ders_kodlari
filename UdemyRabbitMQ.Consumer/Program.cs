using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.Consumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://mgujdhwy:XPjqFTiaVobxvbhF6q3AIK8H4gK251Ke@spider.rmq.cloudamqp.com/mgujdhwy");

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("task_queue", durable: true, exclusive: false, autoDelete: false, null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, false);

                    Console.WriteLine("mesajları beliyorum....");

                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume("task_queue", autoAck: false, consumer);

                    consumer.Received += (model, ea) =>
                         {
                             var message = Encoding.UTF8.GetString(ea.Body);
                             Console.WriteLine("Mesaj alındı:" + message);

                             int time = int.Parse(GetMessage(args));
                             Thread.Sleep(time);
                             Console.WriteLine("Mesaj işlendi...");

                             channel.BasicAck(ea.DeliveryTag, multiple: false);
                         };
                    Console.WriteLine("Çıkış yapmak tıklayınız..");
                    Console.ReadLine();
                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return args[0].ToString();
        }
    }
}