using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

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
                    channel.QueueDeclare("hello", durable: false, exclusive: false, autoDelete: false, null);

                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume("hello", true, consumer);

                    consumer.Received += (model, ea) =>
                         {
                             var message = Encoding.UTF8.GetString(ea.Body);

                             Console.WriteLine("Mesaj alındı:" + message);
                         };
                    Console.WriteLine("Çıkış yapmak tıklayınız..");
                    Console.ReadLine();
                }
            }
        }
    }
}