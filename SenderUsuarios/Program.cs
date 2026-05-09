using RabbitMQ.Client;
using Shared.Models;
using System.Text;
using System.Text.Json;

Console.WriteLine("INICIANDO SENDER DE USUÁRIOS...");

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
    queue: "queue.usuarios.validation",
    durable: false,
    exclusive: false,
    autoDelete: false
);

await channel.QueueBindAsync(
    queue: "queue.usuarios.validation",
    exchange: "hortifruti.exchange.input",
    routingKey: "usuarios.input"
);

var usuario = new UsuarioMessage
{
    NomeCompleto = "Maria Eduarda Araujo",
    Endereco = "Rua das Flores, 100",
    RG = "12.345.678-9",
    CPF = "123.456.789-00",
    DataRegistro = DateTime.Now
};

var mensagem = JsonSerializer.Serialize(usuario);

var body = Encoding.UTF8.GetBytes(mensagem);

await channel.BasicPublishAsync(
    exchange: "hortifruti.exchange.input",
    routingKey: "usuarios.input",
    body: body
);

Console.WriteLine("USUÁRIO ENVIADO COM SUCESSO!");
Console.WriteLine(mensagem);