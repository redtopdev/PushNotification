using Engaze.Core.DataContract;

namespace Notification.DataContract
{
    public class EventNotification
    {
        public Event Event { get; set; }
        public OccuredEventType NotificationType { get; set; }
    }
}
