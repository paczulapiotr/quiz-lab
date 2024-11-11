using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Quiz.CommonLib.MessageBroker.Filter;

internal class MessageBrokerStartupFilter : IStartupFilter
{
    private readonly string _exchangeName;
    private readonly string _queueName;
    private readonly string _exchangeType;
    private readonly string _routingKey;

    public MessageBrokerStartupFilter(string exchangeName, string queueName, string exchangeType, string routingKey = "")
    {
        _exchangeName = exchangeName;
        _queueName = queueName;
        _exchangeType = exchangeType;
        _routingKey = routingKey;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            Task.Run(async () =>
            {
                var connection = app.ApplicationServices.GetRequiredService<IConnection>();
                var channel = await connection.CreateChannelAsync(); ;

                await channel.ExchangeDeclareAsync(exchange: _exchangeName, type: _exchangeType, durable: true, autoDelete: false, arguments: null);
                await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                await channel.QueueBindAsync(queue: _queueName, exchange: _exchangeName, routingKey: _routingKey, arguments: null);
            }).GetAwaiter().GetResult();


            next(app);
        };
    }
}