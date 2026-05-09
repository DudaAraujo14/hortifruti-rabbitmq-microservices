using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Models;
using System.Text;
using System.Text.Json;

Console.WriteLine("VALIDATION SERVICE INICIADO...");

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange: "hortifruti.exchange.input",
    type: ExchangeType.Direct
);

await channel.ExchangeDeclareAsync(
    exchange: "hortifruti.exchange.validated",
    type: ExchangeType.Direct
);

await channel.QueueDeclareAsync(
    queue: "queue.frutas.validation",
    durable: false,
    exclusive: false,
    autoDelete: false
);

await channel.QueueDeclareAsync(
    queue: "queue.usuarios.validation",
    durable: false,
    exclusive: false,
    autoDelete: false
);

await channel.QueueDeclareAsync(
    queue: "queue.frutas.receiver",
    durable: false,
    exclusive: false,
    autoDelete: false
);

await channel.QueueDeclareAsync(
    queue: "queue.usuarios.receiver",
    durable: false,
    exclusive: false,
    autoDelete: false
);

await channel.QueueBindAsync(
    queue: "queue.frutas.validation",
    exchange: "hortifruti.exchange.input",
    routingKey: "frutas.input"
);

await channel.QueueBindAsync(
    queue: "queue.usuarios.validation",
    exchange: "hortifruti.exchange.input",
    routingKey: "usuarios.input"
);

await channel.QueueBindAsync(
    queue: "queue.frutas.receiver",
    exchange: "hortifruti.exchange.validated",
    routingKey: "frutas.validated"
);

await channel.QueueBindAsync(
    queue: "queue.usuarios.receiver",
    exchange: "hortifruti.exchange.validated",
    routingKey: "usuarios.validated"
);

var frutasConsumer = new AsyncEventingBasicConsumer(channel);

frutasConsumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();

    var mensagem = Encoding.UTF8.GetString(body);

    Console.WriteLine("\nFRUTA RECEBIDA:");
    Console.WriteLine(mensagem);

    var fruta = JsonSerializer.Deserialize<FrutaMessage>(mensagem);

    if (fruta != null &&
        !string.IsNullOrEmpty(fruta.NomeFruta) &&
        !string.IsNullOrEmpty(fruta.Descricao))
    {
        Console.WriteLine("FRUTA VALIDADA!");

        var jsonValidado = JsonSerializer.Serialize(fruta);

        var bodyValidado = Encoding.UTF8.GetBytes(jsonValidado);

        await channel.BasicPublishAsync(
            exchange: "hortifruti.exchange.validated",
            routingKey: "frutas.validated",
            body: bodyValidado
        );
    }
};

var usuariosConsumer = new AsyncEventingBasicConsumer(channel);

usuariosConsumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();

    var mensagem = Encoding.UTF8.GetString(body);

    Console.WriteLine("\nUSUÁRIO RECEBIDO:");
    Console.WriteLine(mensagem);

    var usuario = JsonSerializer.Deserialize<UsuarioMessage>(mensagem);

    if (usuario != null &&
        !string.IsNullOrEmpty(usuario.NomeCompleto) &&
        !string.IsNullOrEmpty(usuario.CPF))
    {
        Console.WriteLine("USUÁRIO VALIDADO!");

        var jsonValidado = JsonSerializer.Serialize(usuario);

        var bodyValidado = Encoding.UTF8.GetBytes(jsonValidado);

        await channel.BasicPublishAsync(
            exchange: "hortifruti.exchange.validated",
            routingKey: "usuarios.validated",
            body: bodyValidado
        );
    }
};

await channel.BasicConsumeAsync(
    queue: "queue.frutas.validation",
    autoAck: true,
    consumer: frutasConsumer
);

await channel.BasicConsumeAsync(
    queue: "queue.usuarios.validation",
    autoAck: true,
    consumer: usuariosConsumer
);

Console.WriteLine("AGUARDANDO MENSAGENS...");
Console.ReadLine();