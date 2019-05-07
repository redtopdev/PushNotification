namespace Engaze.Evento.Pushnotification.Contract
{
    public class InviteEvent : Evento
    {
        public InviteEvent(string initiatorId, string initiatorName, string eventId, string eventName, string eventResponderId, string eventResponderName) : base(eventId, eventName, EventoEventType.EventoInvited)
        {
            this.InitiatorId = initiatorId;
            this.InitiatorName = initiatorName;
        }
        public string InitiatorId { get; private set; }
        public string InitiatorName { get; private set; }
    }
}
