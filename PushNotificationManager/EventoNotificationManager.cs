using Pushnotification.Contract;
using System.Collections.Generic;

namespace PushNotification.Manager
{
    public class EventoNotificationManager : INotificationManager
    {
        private IPushNotifier Notifier;
        public EventoNotificationManager(IPushNotifier notifier)
        {
            this.Notifier = notifier;
        }
        public void NotifyEventUpdate(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, PushMessageType.EventoUpdated, registrationIds);
        }
        public void NotifyEventUpdateParticipants(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, PushMessageType.EventParticipantsUpdated, registrationIds);
        }
        public void NotifyEventUpdateLocation(string eventId, string eventName, List<string> registrationIds)
        {
           
        }
        public void NotifyAddParticipantToEvent(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, PushMessageType.EventoInvited, registrationIds);
        }
        public void NotifyRemoveParticipantFromEvent(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, PushMessageType.RemovedFromEvento, registrationIds);
        }
        public void NotifyEndEvent(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, PushMessageType.EventoEnded, registrationIds);
        }
        public void NotifyExtendEvent(string eventId, string eventName, List<string> registrationIds)
        {
        }

        public void NotifyDeleteEvent(string eventId, string eventName, List<string> registrationIds)
        {
            this.NotifyEvent(eventId, eventName, PushMessageType.EventoDeleted, registrationIds);
        }
        public void NotifyLeftEvent(string eventId, string eventName, List<string> registrationIds)
        {
            
        }
        public void NotifyAdditionalRegisteredUserInfoToHost() { }
        public void NotifyEventResponse() { }

        public void NotifyRemindContact() { }

        private void NotifyEvent(string eventId, string eventName, string eventType, List<string> registrationIds)
        {
            this.Notifier.Notify<Pushnotification.Contract.Event>(registrationIds,
               new Pushnotification.Contract.Event(eventId, eventName, eventType));
        }

        public void NotifyEventCreated(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyEventDeleted(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyEventEnded(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyEventUpdated(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyEventLocationUpdated(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyEventExtended(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyParticipantRemoved(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyParticipantAddded(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyParticipantLeftEvent(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyParticipantStateUpdated(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyParticipantListUpdated(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyDestinationUpdated(NotificationData notificationData)
        {
            throw new System.NotImplementedException();
        }
    }
}
