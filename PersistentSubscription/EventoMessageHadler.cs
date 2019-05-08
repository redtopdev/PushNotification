using Engaze.Core.MessageBroker.Consumer;
using Engaze.Evento.PushNotification.Manager;
using Engaze.Evento.PushNotification.Persistance;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Engaze.Evento.PushNotification.Service
{
    public class EventoMessageHadler : IMessageHandler
    {
        private IDataRepository repo;
        INotificationManager notificationManager;

        public EventoMessageHadler(IDataRepository repository, INotificationManager eventoNotificationManager)
        {
            this.repo = repository;
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
                await ProcessMessage(JObject.Parse(message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception ocured :" + ex.ToString());
            }
        }

        private async Task ProcessMessage(JObject msgJObject)
        {
            string eventType = msgJObject.Value<string>("EventType");
            JObject eventoObject = msgJObject.Value<JObject>("Data");
            string eventId = eventoObject.Value<string>("EventoId");
            var userIds = (await repo.GetAffectedUserIdList(Guid.Parse(eventId))).ToList<string>();

            switch (eventType)
            {
                case "EventoCreated":
                    ////notificationManager.NotifyEventCreation((eventId.ToString(), null, userIds);
                    break;
                case "EventoDeleted":
                    notificationManager.NotifyDeleteEvent(eventId.ToString(), null, userIds);                  
                    break;

                case "EventoEnded":
                    notificationManager.NotifyEndEvent(eventId.ToString(), null, userIds);
                    break;

                case "EventoExtended":
                    notificationManager.NotifyExtendEvent(eventId.ToString(), null, userIds);
                    break;

                case "ParticipantLeft":
                    notificationManager.NotifyExtendEvent(eventId.ToString(), null, userIds);
                    break;

                case "ParticipantsListUpdated":
                    notificationManager.NotifyExtendEvent(eventId.ToString(), null, userIds);
                    break;

                case "ParticipantStateUpdated":
                    notificationManager.NotifyExtendEvent(eventId.ToString(), null, userIds);
                    break;

                default:
                    break;

            }
        }
    }
}