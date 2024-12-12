
namespace dotnet_mvc.RabbitMQ
{
    public interface IRabbmitMQCartMessageSender
    {
        void SendMessage(object message, string queueName);
    }
}