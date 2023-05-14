using CartingService.BLL.Interfaces;
using CartingService.DAL.Entities;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;

namespace CartingService.Api.BackgroundServices
{
    public class ItemChangesQueueBackgroundService: BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;
        public ConnectionFactory connectionFactory;
        public IModel channel;
        private EventingBasicConsumer consumer;

        public ItemChangesQueueBackgroundService(IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.configuration = configuration;

            connectionFactory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMq:HostName"],
      //          UserName = configuration["RabbitMq:UserName"],
      //          Password = configuration["RabbitMq:Password"]
            };
            var connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(
                                 queue: configuration["RabbitMq:ItemQueueName"],
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null
                                 );
            consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: configuration["RabbitMq:ItemQueueName"], autoAck: true, consumer: consumer);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            while (!stoppingToken.IsCancellationRequested)
            {
                consumer.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();

                    var jsonBody = Encoding.UTF8.GetString(body);
                    Item message = JsonConvert.DeserializeObject<Item>(jsonBody);

                    using (IServiceScope scope = serviceProvider.CreateScope())
                    {
                        ICartService cartService =
                            scope.ServiceProvider.GetRequiredService<ICartService>();

                        cartService.UpdateItem(message);
                    }
                };

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }

}
