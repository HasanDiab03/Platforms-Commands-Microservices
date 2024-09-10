using Microsoft.Extensions.Options;
using PlatformService.DTOs;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
	public class MessageBusClient : IMessageBusClient
	{
		private readonly RabbitMqConfig rabbitMqConfig;
		private readonly IConnection _connection;
		private readonly IModel _channel;
		private readonly string _exchangeName = "trigger";

		public MessageBusClient(IConfiguration config, IOptions<RabbitMqConfig> options)
        {
			rabbitMqConfig = options.Value;
			// this is what is used to establish and build the connection to the rabbitMq server
			var factory = new ConnectionFactory()
			{
				HostName = rabbitMqConfig.Host,
				Port = int.Parse(rabbitMqConfig.Port)
			};
			try
			{
				// this is the connection that we are going to use to publish messages to
				_connection = factory.CreateConnection();
				// this is the channel that we are going to use to publish messages to
				_channel = _connection.CreateModel();
				// this is the exchange that we are going to use to publish messages to
				_channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);
				// this is the event that is triggered when the connection is shutdown
				_connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
				Console.WriteLine("--> Connected to Message Bus");
			}
			catch (Exception ex) 
			{
				Console.WriteLine($"--> Failed to connect to RabbitMQ: {ex.Message}");
			}
        }
        public void PublishNewPlatform(PlatformPublishDTO platform)
		{
			var message = JsonSerializer.Serialize(platform);
			if (!_connection.IsOpen)
			{
				Console.WriteLine("---> No Connection to rabbitMq is open right now");
				return;
			}
			Console.WriteLine("---> Sending message to RabbitMQ");
			SendMessage(message);
		}
		private void SendMessage(string message)
		{
			var body = Encoding.UTF8.GetBytes(message);
			// no routingKey, since this is a fanout type
			_channel.BasicPublish(exchange: _exchangeName,
			 	routingKey: "", basicProperties: null, body: body);
			Console.WriteLine($"---> message: {message} has been sent");
		}

		public void Dispose()
		{
			Console.WriteLine("--> Disposing of RabbitMQ");
			if(_channel.IsOpen)
			{
				_channel.Close();
				_connection.Close();
			}
		}

		private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
		{
			Console.WriteLine("--> RabbitMQ Connection Shutdown");
		}
	}
}
