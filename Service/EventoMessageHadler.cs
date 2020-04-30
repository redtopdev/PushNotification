using Engaze.Core.DataContract;
using Engaze.Core.MessageBroker.Consumer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pushnotification.Contract;
using PushNotification.Manager;
using PushNotification.Persistance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PushNotification.Service
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
            var eventType = msgJObject.Value<OccuredEventType>("EventType");
            JObject eventoObject = msgJObject.Value<JObject>("Data");
            Guid eventId = eventoObject.Value<Guid>("EventId");
            var userIds = eventoObject.Value<IEnumerable<Guid>>("userIds");
            NotificationData notificationData = null;

            switch (eventType)
            {
                case OccuredEventType.EventoCreated:
                    var evnt = JsonConvert.DeserializeObject(eventoObject.ToString());

                    notificationManager.NotifyEventCreated(notificationData);
                    break;

                case OccuredEventType.EventoUpdated:                 
                    notificationManager.NotifyEventUpdated(CreateMessage(eventoObject, PushMessageType.EventUpdate));
                    break;

                case OccuredEventType.EventoDeleted:                    
                    notificationManager.NotifyEventDeleted(CreateMessage(eventoObject, PushMessageType.EventDeleted));
                    break;

                case OccuredEventType.EventoEnded:
                    notificationManager.NotifyEventDeleted(CreateMessage(eventoObject, PushMessageType.EventEnd));
                    break;

                case OccuredEventType.EventoExtended:
                    notificationManager.NotifyEventDeleted(CreateMessage(eventoObject, PushMessageType.EventExtend));
                    break;

                case OccuredEventType.ParticipantLeft:
                    notificationManager.NotifyParticipantLeftEvent(notificationData);
                    break;

                case OccuredEventType.ParticipantAdded:
                    notificationManager.NotifyParticipantAddded(notificationData);
                    break;

                case OccuredEventType.ParticipantRemoved:
                    notificationManager.NotifyParticipantRemoved(notificationData);
                    break;

                case OccuredEventType.ParticipantStateUpdated:
                    notificationManager.NotifyParticipantStateUpdated(notificationData);
                    break;

                case OccuredEventType.ParticipantsListUpdated:
                    notificationManager.NotifyParticipantListUpdated(notificationData);
                    break;

                default:
                    break;

            }


            //void NotifyEventUpdateLocation(EventRequestSlim eventRequest, EventSlim evt);         

            //void NotifyEventResponse(EventRequestSlim eventRequest, EventSlim evt);
            //void NotifyAdditionalRegisteredUserInfoToHost(List<EventParticipantSlim> additionalRegisteredUsers, EventSlim evt);
            //void NotifyRemindContact(RemindRequestSlim remindReq);
        }

        private NotificationData CreateMessage(JObject eventoObject, PushMessageType messageType)
        {
            return new NotificationData(
                       eventoObject.Value<Guid>("EventId"), eventoObject.Value<string>("EventName"),
                       eventoObject.Value<Guid>("InitiatorId"), eventoObject.Value<string>("InitiatorNamr"),
                       messageType, eventoObject.Value<IEnumerable<Guid>>("userIds"));
        }
    }
}