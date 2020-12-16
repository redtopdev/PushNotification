using Engaze.Core.DataContract;
using Newtonsoft.Json.Linq;
using Notification.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.Manager
{
    public class EventoNotificationManager : INotificationManager
    {
        private readonly IPushNotifier notifier;
        private readonly IUserProfileClient userProfileClient;

        public EventoNotificationManager(IPushNotifier notifier, IUserProfileClient userProfileClient)
        {
            this.notifier = notifier;
            this.userProfileClient = userProfileClient;
        }

        public async Task HandleEvent(JObject eventNotificationJObj)
        {
            NotificationData notficationData;
            var eventObject = eventNotificationJObj.Value<JObject>("Event");
            var userIds = GetEventParticipants(eventObject);


            OccuredEventType eventType = ParseMyEnum<OccuredEventType>(eventNotificationJObj.Value<string>("NotificationType"));


            switch (eventType)
            {
                case OccuredEventType.EventoCreated:
                    notficationData = CreateMessage(eventObject, PushMessageType.EventInvite);
                    userIds.ToList().Remove(eventObject.Value<Guid>("InitiatorId"));
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.EventoUpdated:
                    await NotifyEventAndParticipantListUpdated(eventObject, userIds);
                    userIds.ToList().Remove(eventObject.Value<Guid>("InitiatorId"));
                    break;

                case OccuredEventType.EventoDeleted:
                    notficationData = CreateMessage(eventObject, PushMessageType.EventDeleted);
                    userIds.ToList().Remove(eventObject.Value<Guid>("InitiatorId"));
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.EventoEnded:
                    notficationData = CreateMessage(eventObject, PushMessageType.EventEnd);
                    userIds.ToList().Remove(eventObject.Value<Guid>("InitiatorId"));
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.EventoExtended:
                    notficationData = CreateMessage(eventObject, PushMessageType.EventExtend);
                    userIds.ToList().Remove(eventObject.Value<Guid>("InitiatorId"));
                    notficationData.ExtendDuration = eventObject.Value<int>("ExtendDuration");
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.ParticipantLeft:
                    notficationData = CreateMessage(eventObject, PushMessageType.EventLeave);
                    notficationData.ResponderId = eventObject.Value<Guid>("RequesterId");
                    notficationData.ResponderName = eventObject.Value<string>("RequesterName");
                    userIds.ToList().Remove(notficationData.ResponderId.Value);
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.DestinationUpdated:
                    notficationData = CreateMessage(eventObject, PushMessageType.EventUpdateLocation);
                    notficationData.DestinationName = eventObject.Value<string>("DestinationName");
                    userIds.ToList().Remove(notficationData.InitiatorId);
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.ParticipantsListUpdated:
                    await NotifyEventAndParticipantListUpdated(eventObject, userIds);
                    break;

                case OccuredEventType.ParticipantStateUpdated:
                    notficationData = CreateMessage(eventObject, PushMessageType.EventResponse);
                    notficationData.ResponderId = eventObject.Value<Guid>("RequesterId");
                    notficationData.ResponderName = eventObject.Value<string>("RequesterName");
                    notficationData.EventAcceptanceState = ParseMyEnum<EventAcceptanceStatus>(eventObject.Value<string>("CurrentParticipant.acceptanceStatus"));
                    userIds.ToList().Remove(notficationData.ResponderId.Value);
                    await NotifyEvent(notficationData, userIds);
                    break;
            }
        }

        private NotificationData CreateMessage(JObject eventoObject, PushMessageType messageType)
        {
            return new NotificationData(
                       eventoObject.Value<Guid>("EventId"), eventoObject.Value<string>("EventName"),
                       eventoObject.Value<Guid>("InitiatorId"), eventoObject.Value<string>("InitiatorName"),
                       messageType);
        }

        private IEnumerable<Guid> GetEventParticipants(JObject eventObject)
        {
            //return eventObject.Value<IEnumerable<Guid>>("UserIds");
            var list = eventObject["Participants"].ToObject<IEnumerable<Participant>>();
            return list.Select(x => x.UserId);
        }

        private IEnumerable<Guid> GetAddedUserIds(JObject eventObject)
        {
            try
            {
                return eventObject.Value<IEnumerable<Guid>>("AddedUserIds");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public T ParseMyEnum<T>(object o)
        {
            T enumVal = (T)Enum.Parse(typeof(T), o.ToString());
            return enumVal;
        }

        private IEnumerable<Guid> GetRemovedUserIds(JObject eventObject)
        {
            try
            {
                return eventObject.Value<IEnumerable<Guid>>("RemovedUserIds");
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task NotifyEventAndParticipantListUpdated(JObject eventObject, IEnumerable<Guid> userids)
        {
            var addedUserIds = GetAddedUserIds(eventObject);
            var removedUserIds = GetRemovedUserIds(eventObject);
            var remainingUserIds = userids.Except(addedUserIds);

            //Send Invite event notification for the new invited users
            if (addedUserIds != null && addedUserIds.Count() > 0)
            {
                await NotifyEvent(CreateMessage(eventObject, PushMessageType.EventInvite), addedUserIds);
            }
            if (removedUserIds != null && remainingUserIds.Count() > 0)
            {

                await NotifyEvent(CreateMessage(eventObject, PushMessageType.RemovedFromEvent), removedUserIds);
            }

            var notificationDataForEventUpdate = CreateMessage(eventObject, PushMessageType.EventUpdate);
            remainingUserIds.ToList().Remove(notificationDataForEventUpdate.InitiatorId);

            await NotifyEvent(notificationDataForEventUpdate, remainingUserIds);
        }

        private async Task NotifyEvent(NotificationData notificationData, IEnumerable<Guid> userIds)
            =>
            await notifier.Notify(userIds.Select(userId=>userId.ToString()), notificationData, notificationData.MessageType.ToString());           
       
    }
}
