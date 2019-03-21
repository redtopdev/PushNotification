using Engaze.Core.MessageBroker.Consumer;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaListener
{
    public class EventoMessageHadler : IMessageHandler
    {
        public void OnError(string error)
        {
            Console.WriteLine(error);
        }

        public void OnMessageReceived(string message)
        {
            Console.WriteLine(message);
        }
    }
}
