using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.EventHandlers
{
    public interface IMessageProducer
    {
        public void SendingMessage<T>(T message);
    }
}