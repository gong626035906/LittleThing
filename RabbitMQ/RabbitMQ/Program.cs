using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSend
{
    class Program
    {
        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory();
            //factory.HostName = "localhost";//RabbitMQ服务在本地运行
            //factory.UserName = "wumerp";//用户名
            //factory.Password = "wuerp";//密码

            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare("hello", false, false, false, null);//创建一个名称为hello的消息队列
            //        string message = "Hello World"; //传递的消息内容
            //        var body = Encoding.UTF8.GetBytes(message);
            //        channel.BasicPublish("", "hello", null, body); //开始传递
            //        Console.WriteLine("已发送： {0}", message);
            //        Console.ReadLine();
            //    }
            //}

            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "wumerp";
            factory.Password = "wuerp";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //channel.QueueDeclare("hello", false, false, false, null);
                    channel.QueueDeclare("task_queue", true, false, false, null);
                    string message = GetMessage(args);
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;
                    properties.SetPersistent(true);

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", "task_queue", properties, body);
                    //channel.BasicPublish("", "hello", properties, body);
                    Console.WriteLine(" set {0}", message);
                }
            }

            Console.ReadKey();
        }
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
