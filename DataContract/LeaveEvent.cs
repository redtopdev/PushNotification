namespace Pushnotification.Contract
{
    public class LeaveEvent : Event
    {
        public LeaveEvent(string eventId, string eventName, string eventResponderId, string eventResponderName) : base(eventId, eventName, PushMessageType.EventoLeft)
        {
            this.EventResponderId = eventResponderId;
            this.EventResponderName = eventResponderName;
        }

        public string EventResponderId { get; private set; }
        public string EventResponderName { get; private set; }
    }
}
