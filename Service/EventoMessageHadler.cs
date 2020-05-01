using Engaze.Core.MessageBroker.Consumer;
using Newtonsoft.Json.Linq;
using PushNotification.Manager;
using System;
using System.Threading.Tasks;

namespace PushNotification.Service
{
    public class EventoMessageHadler : IMessageHandler
    {

        INotificationManager notificationManager;

        public EventoMessageHadler(INotificationManager eventoNotificationManager)
        {
            this.notificationManager = eventoNotificationManager;
        }
        public void OnError(string error)
        {
            Console.WriteLine(error);
        }

        public async Task OnMessageReceivedAsync(string message)
        {
            try
            {
                await notificationManager.HandleEvent(JObject.Parse(message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception ocured :" + ex.ToString());
            }
        }
    }
}