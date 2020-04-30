namespace Pushnotification.Contract
{
    public class Event : PushData
    {
        public Event(string eventId, string eventName, string type) : base(type)
        {
            this.EventId = eventId;
            this.EventName = eventName;
        }

        public string EventId { get; private set; }
        public string EventName { get; private set; }
    }
}
