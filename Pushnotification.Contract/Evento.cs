using System;

namespace Pushnotification.Contract
{
    public class Evento : PushData
    {
        public Evento(string eventId, string eventName, string type) : base(type)
        {
            this.EventId = eventId;
            this.EventName = eventName;
        }

        public string EventId { get; private set; }
        public string EventName { get; private set; }
    }
}
