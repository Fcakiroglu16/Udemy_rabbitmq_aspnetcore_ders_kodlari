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
                    channel.QueueDeclare("hello", false, false, false, null);

                    string message = "Hello  World";

                    var bodyByte = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", routingKey: "hello", null, body: bodyByte);

                    Console.WriteLine("Mesajınız gönderilmiştir.");
                }

                Console.WriteLine("Çıkış yapmak tıklayınız..");
                Console.ReadLine();
            }
        }
    }
}