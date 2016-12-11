using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RxSample_NetCore
{
    public interface IMessage : IDisposable
    {
        string Id { get; }
        Task CompleteAsync();
        Task AbandonAsync();
    }

    public interface IMessage<TMessage> : IMessage
    {
        TMessage GetContent();
    }

    internal class SimpleRabbitMessage : IMessage<string>
    {
        private readonly IModel _channel;
        private readonly BasicDeliverEventArgs _messageArgs;
        private bool _ackedOrNacked = false;

        public SimpleRabbitMessage(IModel channel, BasicDeliverEventArgs messageArgs)
        {
            if(channel == null) throw new ArgumentNullException(nameof(channel));
            if(messageArgs == null) throw new ArgumentNullException(nameof(messageArgs));

            _channel = channel;
            _messageArgs = messageArgs;
        }

        public string Id => _messageArgs.BasicProperties.MessageId;
        public string GetContent() => Encoding.UTF8.GetString(_messageArgs.Body);

        public Task CompleteAsync()
        {
            _channel.BasicAck(_messageArgs.DeliveryTag, multiple: false);
            _ackedOrNacked = true;

            return Task.CompletedTask;
        }

        public Task AbandonAsync()
        {
            AbandonImpl();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if(_ackedOrNacked == false) 
            {
                AbandonImpl();
            }
        }

        private void AbandonImpl() 
        {
            _channel.BasicNack(_messageArgs.DeliveryTag, multiple: false, requeue: true);
            _ackedOrNacked = true;
        }
    }
}