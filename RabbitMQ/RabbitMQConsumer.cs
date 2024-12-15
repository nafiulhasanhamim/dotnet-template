// using System.Text;
// using System.Text.Json;
// using dotnet_mvc.DTOs;
// using dotnet_mvc.Interfaces;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;

// namespace dotnet_mvc.RabbitMQ
// {
//     public class RabbitMQConsumer : BackgroundService
//     {
//         private readonly IServiceProvider _serviceProvider;
//         private IConnection _connection;
//         private IModel _channel;

//         public RabbitMQConsumer(IServiceProvider serviceProvider)
//         {
//             _serviceProvider = serviceProvider;

//             var factory = new ConnectionFactory
//             {
//                 HostName = "localhost",
//                 UserName = "guest",
//                 Password = "guest"
//             };

//             _connection = factory.CreateConnection();
//             _channel = _connection.CreateModel();
//             _channel.QueueDeclare("orderCreateds", false, false, false, null); // Queue for product update events
//         }

//         protected override Task ExecuteAsync(CancellationToken stoppingToken)
//         {
//             stoppingToken.ThrowIfCancellationRequested();

//             CreateConsumer("orderCreateds", async (message) =>
//             {
//                 Console.WriteLine("ordercreates event from productapi");

//                 var eventMessage = JsonSerializer.Deserialize<EventDTO>(message);
//                 using (var scope = _serviceProvider.CreateScope())
//                 {
//                     var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
//                     await orderService.NotificationHandling(eventMessage);
//                 }

//             });

//             return Task.CompletedTask;
//         }

//         private void CreateConsumer(string queueName, Func<string, Task> processMessage)
//         {
//             var consumer = new EventingBasicConsumer(_channel);

//             consumer.Received += async (ch, ea) =>
//             {
//                 var body = ea.Body.ToArray();
//                 var message = Encoding.UTF8.GetString(body);
//                 await processMessage(message);
//                 _channel.BasicAck(ea.DeliveryTag, false);
//             };

//             _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
//         }
//         public override void Dispose()
//         {
//             _channel.Close();
//             _connection.Close();
//             base.Dispose();
//         }
//     }
// }


using System.Text;
using System.Text.Json;
using dotnet_mvc.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace dotnet_mvc.RabbitMQ
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "OrderExchange", type: ExchangeType.Fanout);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "OrderExchange", routingKey: "");

            CreateConsumer(queueName, async (message) =>
            {
                var eventMessage = JsonSerializer.Deserialize<EventDTO>(message);
            });

            return Task.CompletedTask;
        }

        private void CreateConsumer(string queueName, Func<string, Task> processMessage)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await processMessage(message);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
