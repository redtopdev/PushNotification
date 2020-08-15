using Engaze.Core.DataContract;
using System;

namespace Notification.DataContract
{
    public class NotificationData
    {
        public Guid EventId { get; set; }

        public string EventName { get; set; }

        public Guid InitiatorId { get; set; }

        public string InitiatorName { get; set; }

        public Guid? ResponderId { get; set; }

        public string ResponderName { get; set; }

        public EventAcceptanceStatus EventAcceptanceState { get; set; }

        public string DestinationName { get; set; }

        public PushMessageType MessageType { get; set; }

        public int? ExtendDuration { get; set; }      

        public NotificationData(Guid eventId, string eventName, Guid initiatorId, string initiatorName,
            PushMessageType messageType)
        {
            this.EventId = eventId;
            this.EventName = eventName;
            this.InitiatorId = initiatorId;
            this.InitiatorName = initiatorName;
            this.MessageType = messageType;
        
        }
    }
}
