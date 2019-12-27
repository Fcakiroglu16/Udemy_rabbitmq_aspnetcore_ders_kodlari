using RabbitMQ.Client;
using System;
using System.Text;

namespace UdemyRabbitMQ.Publisher
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
                    channel.QueueDeclare("task_queue", durable: true, false, false, null);

                    string message = GetMessage(args);

                    for (int i = 1; i < 11; i++)
                    {
                        var bodyByte = Encoding.UTF8.GetBytes($"{message}-{i}");

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = true;

                        channel.BasicPublish("", routingKey: "task_queue", properties, body: bodyByte);

                        Console.WriteLine($"Mesajınız gönderilmiştir:{message}-{i}");
                    }
                }

                Console.WriteLine("Çıkış yapmak tıklayınız..");
                Console.ReadLine();
            }
        }

        private static string GetMessage(string[] args)
        {
            return args[0].ToString();
        }
    }
}