using System;
using System.Collections.Generic;
using System.Text;

namespace Pushnotification.Contract
{
    public class LeaveEvent : Evento
    {
        public LeaveEvent(string eventId, string eventName, string eventResponderId, string eventResponderName) : base(eventId, eventName, EventoEventType.EventoLeft)
        {
            this.EventResponderId = eventResponderId;
            this.EventResponderName = eventResponderName;
        }

        public string EventResponderId { get; private set; }
        public string EventResponderName { get; private set; }
    }
}
