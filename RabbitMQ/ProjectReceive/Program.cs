using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory();
            //factory.HostName = "localhost";
            //factory.UserName = "wumerp";
            //factory.Password = "wuerp";

            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare("hello", false, false, false, null);

            //        var consumer = new EventingBasicConsumer(channel);
            //        channel.BasicConsume("hello", false, consumer);
            //        consumer.Received += (model, ea) =>
            //        {
            //            var body = ea.Body;
            //            var message = Encoding.UTF8.GetString(body);
            //            Console.WriteLine("已接收： {0}", message);
            //            channel.BasicAck(ea.DeliveryTag, false);
            //        };
            //        Console.ReadLine();
            //    }
            //}

            var factory = new ConnectionFactory();
            factory.HostName = "192.168.22.76";
            factory.UserName = "wumerp";
            factory.Password = "wuerp";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //channel.QueueDeclare("hello", false, false, false, null);
                    channel.QueueDeclare("task_queue", true, false, false, null);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    //channel.BasicConsume("hello", true, consumer);
                    channel.BasicConsume("task_queue", false, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        int dots = message.Split('.').Length - 1;
                        Thread.Sleep(dots * 1000);

                        Console.WriteLine("Received {0}", message);
                        Console.WriteLine("Done");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            }
        }
    }
}
