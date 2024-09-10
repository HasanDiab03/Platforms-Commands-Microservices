
using System.Text;
using CommandService.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices
{
    // This is a background service that will listen to the message bus and process messages
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly RabbitMqConfig _rabbitMqConfig;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IEventProcessor eventProcessor, IOptions<RabbitMqConfig> options)
        {
            _eventProcessor = eventProcessor;
            _rabbitMqConfig = options.Value;
            EstablishConnection();
        }

        private void EstablishConnection()
        {
            var factory = new ConnectionFactory() 
            {
                HostName = _rabbitMqConfig.Host,
                Port = int.Parse(_rabbitMqConfig.Port)
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _rabbitMqConfig.Exchange, type: ExchangeType.Fanout);
            // since i am using QueueDeclare without any parameters, it will create a random queue name and dispose of it when the connection is closed (temporary queue)
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName, exchange: _rabbitMqConfig.Exchange,
             routingKey: "");
            Console.WriteLine("--> Listening on rabbitmq");
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ModuleHandle, ea) => 
            {
                Console.WriteLine("--> Event Received");
                var body = ea.Body;
                // convert from byte array to string
                var message = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(message);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
		{
			Console.WriteLine("--> RabbitMQ Connection Shutdown");
		}
        public override void Dispose()
        {
            Console.WriteLine("--> MessageBus Disposed");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}