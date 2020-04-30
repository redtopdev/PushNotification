using System;
using System.Collections.Generic;

namespace Pushnotification.Contract
{
    public class NotificationData
    {
        public Guid EventId { get; private set; }

        public string EventName { get; private set; }

        public Guid InitiatorId { get; private set; }

        public string InitiatorName { get; private set; }

        public PushMessageType MessageType { get; private set; }

        public IEnumerable<Guid> UserIds { get; private set; }

        public NotificationData(Guid eventId, string eventName, Guid initiatorId, string initiatorName,
            PushMessageType messageType, IEnumerable<Guid> userIds)
        {
            this.EventId = eventId;
            this.EventName = eventName;
            this.InitiatorId = initiatorId;
            this.InitiatorName = initiatorName;
            this.MessageType = messageType;
            this.UserIds = userIds;
        }
    }
}
