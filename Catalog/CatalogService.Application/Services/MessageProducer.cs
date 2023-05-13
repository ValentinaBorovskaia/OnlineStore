using CatalogService.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CatalogService.Application.Services
{
    public class MessageProducer : IMessageProducer
    {
        private readonly ConnectionFactory connectionFactory;
        private readonly RabbitMQ.Client.IModel channel;
        public MessageProducer(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
            var connection = this.connectionFactory.CreateConnection();
            channel = connection.CreateModel();

        }
        public void SendMessage<T>(string queueName, T message)
        {
            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false);
            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            channel.BasicPublish("", queueName, body: body);

        }

    }
}
