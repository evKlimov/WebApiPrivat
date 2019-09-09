using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using WebApi.ApplicationLayer.Helpers;
using WebApi.ApplicationLayer.Rabbit;

namespace WebApi.ApplicationLayer.RabbitMQ
{
    public class RpcClient : IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<byte[]> respQueue = new BlockingCollection<byte[]>();
        private readonly IBasicProperties props;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(body);
                }
            };
        }

        public object Call<T>(T obj)
        {
            channel.BasicPublish(
                exchange: "",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: new RabbitMessage() { Data = obj.ToBytes() }.ToBytes());

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            return respQueue.Take().FromBytes();
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
