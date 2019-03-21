using Pushnotification.Contract;
using System.Collections.Generic;

namespace PushNotification
{
    public class EventoNotificationManager
    {
        private IPushNotifier Notifier;
        public void NotifyEventCreation(IPushNotifier notifier)
        {
            this.Notifier = notifier;
        }
        public void NotifyEventUpdate(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, EventoEventType.EventoUpdated, registrationIds);
        }
        public void NotifyEventUpdateParticipants(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, EventoEventType.EventParticipantsUpdated, registrationIds);
        }
        public void NotifyEventUpdateLocation(string eventId, string eventName, List<string> registrationIds)
        {
           
        }
        public void NotifyAddParticipantToEvent(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, EventoEventType.EventoInvited, registrationIds);
        }
        public void NotifyRemoveParticipantFromEvent(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, EventoEventType.RemovedFromEvento, registrationIds);
        }
        public void NotifyEndEvent(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, EventoEventType.EventoEnded, registrationIds);
        }
        public void NotifyExtendEvent() { }
        public void NotifyDeleteEvent(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, EventoEventType.EventoDeleted, registrationIds);
        }
        public void NotifyLeftEvent(string eventId, string eventName, List<string> registrationIds)
        {
            
        }
        public void NotifyAdditionalRegisteredUserInfoToHost() { }
        public void NotifyEventResponse() { }

        public void NotifyRemindContact() { }

        public void NotifyEvent(string eventId, string eventName, string eventType, List<string> registrationIds)
        {
            this.Notifier.Notify<Evento>(registrationIds,
               new Evento(eventId, eventName, eventType));
        }
    }
}
