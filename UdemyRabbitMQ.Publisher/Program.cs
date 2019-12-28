using RabbitMQ.Client;
using System;
using System.Text;

namespace UdemyRabbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Info = 3,
        Warning = 4
    }

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
                    channel.ExchangeDeclare("direct-exchange", durable: true, type: ExchangeType.Direct);

                    Array log_name_array = Enum.GetValues(typeof(LogNames));

                    for (int i = 1; i < 11; i++)
                    {
                        Random rnd = new Random();

                        LogNames log = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));

                        var bodyByte = Encoding.UTF8.GetBytes($"log={log.ToString()}");

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = true;

                        channel.BasicPublish("direct-exchange", routingKey: log.ToString(), properties, body: bodyByte);

                        Console.WriteLine($"log mesajı gönderilmiştir:{log.ToString()}");
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