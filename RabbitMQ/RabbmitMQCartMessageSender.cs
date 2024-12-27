// using System.Text;
// using Newtonsoft.Json;
// using RabbitMQ.Client;

// namespace dotnet_mvc.RabbitMQ
// {
//     public class RabbmitMQCartMessageSender : IRabbmitMQCartMessageSender
//     {
//         private readonly string _hostName;
//         private readonly string _username;
//         private readonly string _password;
//         private IConnection _connection;

//         public RabbmitMQCartMessageSender()
//         {
//             _hostName = "localhost";
//             _password = "guest";
//             _username = "guest";
//         }

//         public void SendMessage(object message, string exchangeName = "DefaultExchange")
//         {
//             if (string.IsNullOrEmpty(exchangeName))
//             {
//                 throw new ArgumentException("Exchange name cannot be null or empty.", nameof(exchangeName));
//             }

//             if (ConnectionExists())
//             {
//                 using var channel = _connection.CreateModel();

//                 // Dynamically declare the exchange
//                 channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

//                 // Serialize the message to JSON
//                 var json = JsonConvert.SerializeObject(message);
//                 var body = Encoding.UTF8.GetBytes(json);

//                 // Publish the message to the dynamic exchange
//                 channel.BasicPublish(
//                     exchange: exchangeName,
//                     routingKey: "", // Routing key is ignored for fanout exchanges
//                     basicProperties: null,
//                     body: body
//                 );

//             }
//         }

//         private void CreateConnection()
//         {
//             try
//             {
//                 var factory = new ConnectionFactory
//                 {
//                     HostName = _hostName,
//                     Password = _password,
//                     UserName = _username
//                 };

//                 _connection = factory.CreateConnection();
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Could not create connection: {ex.Message}");
//             }
//         }

//         private bool ConnectionExists()
//         {
//             if (_connection != null)
//             {
//                 return true;
//             }

//             CreateConnection();
//             return _connection != null;
//         }
//     }
// }



using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace dotnet_mvc.RabbitMQ
{
    public class RabbmitMQCartMessageSender : IRabbmitMQCartMessageSender
    {
        private readonly string _hostName;
        private readonly string _username;
        private readonly string _password;
        private IConnection _connection;

        public RabbmitMQCartMessageSender(IConfiguration configuration)
        {
            _hostName = configuration.GetSection("RabbitMQ")["HostName"];
            _password = configuration.GetSection("RabbitMQ")["password"];
            _username = configuration.GetSection("RabbitMQ")["guest"];
        }

        public void SendMessage(object message, string name, string type)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name cannot be null or empty.", nameof(name));
            }

            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                if (type == "exchange")
                {
                    channel.ExchangeDeclare(exchange: name, type: ExchangeType.Fanout);

                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(
                        exchange: name,
                        routingKey: "", // Routing key is ignored for fanout exchanges
                        basicProperties: null,
                        body: body
                    );
                    Console.WriteLine($"Message sent to {type} {name}: {json}");
                }
                else if (type == "queue")
                {
                    channel.QueueDeclare(name, false, false, false, null);
                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish(exchange: "", routingKey: name, null, body: body);
                }

            }
        }
        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    Password = _password,
                    UserName = _username
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();
            return _connection != null;
        }
    }
}

