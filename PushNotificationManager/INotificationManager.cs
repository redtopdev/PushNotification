using Pushnotification.Contract;
using System.Collections.Generic;

namespace PushNotification.Manager
{
    public interface INotificationManager
    {
        void NotifyAdditionalRegisteredUserInfoToHost();
        void NotifyEventCreated(NotificationData notificationData);    
        void NotifyEventDeleted(NotificationData notificationData);
        void NotifyEventEnded(NotificationData notificationData);
        void NotifyEventResponse();
        void NotifyEventUpdated(NotificationData notificationData);
        void NotifyEventLocationUpdated(NotificationData notificationData);       
        void NotifyEventExtended(NotificationData notificationData);     
        void NotifyRemindContact();
        void NotifyParticipantRemoved(NotificationData notificationData);      
        void NotifyParticipantAddded(NotificationData notificationData);
        void NotifyParticipantLeftEvent(NotificationData notificationData);
        void NotifyParticipantStateUpdated(NotificationData notificationData);
        void NotifyParticipantListUpdated(NotificationData notificationData);
        void NotifyDestinationUpdated(NotificationData notificationData);
    }
}