using Engaze.Core.MessageBroker.Consumer;
using System;
using System.Threading.Tasks;

namespace Engaze.Evento.PushNotification.Service
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

        public Task OnMessageReceivedAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
