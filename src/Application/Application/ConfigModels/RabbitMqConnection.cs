using System;

namespace Application.ConfigModels;

public class RabbitMqConnection
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public string ReceiveEndpoint { get; set; }

    public Uri HostAddress
    {
        get { return new Uri($"rabbitmq://{Username}:{Password}@{Host}:{Port}/{VirtualHost}"); }
    }
}