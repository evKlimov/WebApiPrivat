using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WebApi.ApplicationLayer;
using WebApi.ApplicationLayer.Executors;
using WebApi.ApplicationLayer.Executors.Commands;
using WebApi.ApplicationLayer.Helpers;
using WebApi.ApplicationLayer.Rabbit;
using WebApi.Repository;

namespace RabbitMQApi
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                  autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    var executor = _serviceProvider.GetRequiredService<RabbitExecutor>();
                    byte[] response = null;
                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var obj = (RabbitMessage)body.FromBytes();

                        var param = obj.Data.FromBytes();
                        var executeMethod = executor.GetType().GetMethod("ApplyCommand").MakeGenericMethod(param.GetType());
                        var result = executeMethod.Invoke(executor, new object[] { param });

                        //var result = executor.ApplyCommand((ICommand)obj.Data.FromBytes());
                        Console.WriteLine("command: {0}", obj);

                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream ms = new MemoryStream();
                        bf.Serialize(ms, result);
                        response = ms.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" [.] " + ex.Message);
                        response = null;
                    }
                    finally
                    {
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                          basicProperties: replyProps, body: response);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                          multiple: false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddLogging();
            collection.AddScoped<RabbitExecutor>();
            collection.AddScoped<IRepositoryClient, ClientRepository>();
            collection.AddScoped<ISession>(x => new Session(new SqlConnection("Data Source=USER-PC;Initial Catalog=WebApi;Integrated Security=True;")));
            collection.AddScoped<ExecutorCommandRabbit<ClientCommand>, ClientRabbitExecutor>();
            collection.AddScoped<ExecutorCommandRabbit<RequestClientCommand>, RequestClientRabbitExecutor>();
            collection.AddScoped<ExecutorCommandRabbit<RequestIdCommand>, RequestIdRabbitExecutor>();
            _serviceProvider = collection.BuildServiceProvider();
            var factory = _serviceProvider.GetService<ILoggerFactory>();
            factory.AddNLog();
            factory.ConfigureNLog("nlog.config");
        }
        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
