namespace Application.ConfigModels;

public class RabbitMq
{
    public RabbitMqConnection DataCollectorConnection { get; set; }
    public RabbitMqConnection DefaultConnection { get; set; }
}
