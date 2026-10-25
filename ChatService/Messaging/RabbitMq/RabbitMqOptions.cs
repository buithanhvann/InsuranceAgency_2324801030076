namespace ChatService.Messaging.RabbitMq;

public class RabbitMqOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
}
