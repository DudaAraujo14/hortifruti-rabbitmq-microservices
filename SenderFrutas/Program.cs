using RabbitMQ.Client;
using Shared.Models;
using System.Text;
using System.Text.Json;

Console.WriteLine("INICIANDO SENDER DE FRUTAS...");

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

await channel.QueueDeclareAsync(
    queue: "queue.frutas.validation",
    durable: false,
    exclusive: false,
    autoDelete: false
);

await channel.QueueBindAsync(
    queue: "queue.frutas.validation",
    exchange: "hortifruti.exchange.input",
    routingKey: "frutas.input"
);

var fruta = new FrutaMessage
{
    NomeFruta = "Manga",
    Descricao = "Fruta tropical rica em vitaminas.",
    DataHora = DateTime.Now
};

var mensagem = JsonSerializer.Serialize(fruta);

var body = Encoding.UTF8.GetBytes(mensagem);

await channel.BasicPublishAsync(
    exchange: "hortifruti.exchange.input",
    routingKey: "frutas.input",
    body: body
);

Console.WriteLine("MENSAGEM ENVIADA COM SUCESSO!");
Console.WriteLine(mensagem);