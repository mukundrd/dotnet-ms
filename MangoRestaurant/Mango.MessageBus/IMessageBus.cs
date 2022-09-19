﻿using Mango.Contracts.Messages;

namespace Mango.MessageBus
{
    public interface IMessageBus
    {
        public Task PublishMessage(BaseMessage message, string topicName);
    }
}
