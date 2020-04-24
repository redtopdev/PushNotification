using System.Collections.Generic;

namespace PushNotification.Manager
{
    public interface INotificationManager
    {
        void NotifyAdditionalRegisteredUserInfoToHost();
        void NotifyAddParticipantToEvent(string eventId, string eventName, List<string> registrationIds);
        void NotifyDeleteEvent(string eventId, string eventName, List<string> registrationIds);
        void NotifyEndEvent(string eventId, string eventName, List<string> registrationIds);
        void NotifyEvent(string eventId, string eventName, string eventType, List<string> registrationIds);
        void NotifyEventResponse();
        void NotifyEventUpdate(string eventId, string eventName, List<string> registrationIds);
        void NotifyEventUpdateLocation(string eventId, string eventName, List<string> registrationIds);
        void NotifyEventUpdateParticipants(string eventId, string eventName, List<string> registrationIds);
        void NotifyExtendEvent(string eventId, string eventName, List<string> registrationIds);
        void NotifyLeftEvent(string eventId, string eventName, List<string> registrationIds);
        void NotifyRemindContact();
        void NotifyRemoveParticipantFromEvent(string eventId, string eventName, List<string> registrationIds);
    }
}