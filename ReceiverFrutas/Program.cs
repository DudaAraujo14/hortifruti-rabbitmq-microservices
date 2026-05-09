using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("RECEIVER FRUTAS INICIADO...");

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange: "hortifruti.exchange.validated",
    type: ExchangeType.Direct
);

await channel.QueueDeclareAsync(
    queue: "queue.frutas.receiver",
    durable: false,
    exclusive: false,
    autoDelete: false
);

await channel.QueueBindAsync(
    queue: "queue.frutas.receiver",
    exchange: "hortifruti.exchange.validated",
    routingKey: "frutas.validated"
);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();

    var mensagem = Encoding.UTF8.GetString(body);

    Console.WriteLine("\nMENSAGEM VALIDADA RECEBIDA:");
    Console.WriteLine(mensagem);

    await Task.CompletedTask;
};

await channel.BasicConsumeAsync(
    queue: "queue.frutas.receiver",
    autoAck: true,
    consumer: consumer
);

Console.WriteLine("AGUARDANDO MENSAGENS...");
Console.ReadLine();