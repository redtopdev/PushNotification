using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pushnotification.Contract
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PushMessageType
    {
        EventInvite,
        EventUpdate,
        EventEnd,
        EventExtend,
        EventDeleted,
        EventLeave,
        EventUpdateParticipants,
        EventUpdateLocation,
        RemovedFromEvent,
        RegisteredUserUpdate,
        EventResponse
    }
}
