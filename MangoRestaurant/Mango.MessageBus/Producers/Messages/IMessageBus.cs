using Mango.Contracts.Messages;

namespace Mango.MessageBus.Producers.Messages
{
    public interface IMessageBus
    {
        public Task PublishMessage(BaseMessage message, string topicName);
    }
}
