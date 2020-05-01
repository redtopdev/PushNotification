using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PushNotification.Manager
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
