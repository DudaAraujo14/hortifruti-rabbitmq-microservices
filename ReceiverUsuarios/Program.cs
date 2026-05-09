using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("RECEIVER USUÁRIOS INICIADO...");

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
    queue: "queue.usuarios.receiver",
    durable: false,
    exclusive: false,
    autoDelete: false
);

await channel.QueueBindAsync(
    queue: "queue.usuarios.receiver",
    exchange: "hortifruti.exchange.validated",
    routingKey: "usuarios.validated"
);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();

    var mensagem = Encoding.UTF8.GetString(body);

    Console.WriteLine("\nUSUÁRIO VALIDADO RECEBIDO:");
    Console.WriteLine(mensagem);

    await Task.CompletedTask;
};

await channel.BasicConsumeAsync(
    queue: "queue.usuarios.receiver",
    autoAck: true,
    consumer: consumer
);

Console.WriteLine("AGUARDANDO MENSAGENS...");
Console.ReadLine();