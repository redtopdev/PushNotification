using Engaze.Core.DataContract;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PushNotification.Manager
{
    public class EventoNotificationManager : INotificationManager
    {
        private IPushNotifier Notifier;
        private IUserProfileClient userProfileClient;

        public EventoNotificationManager(IPushNotifier notifier, IUserProfileClient userProfileClient)
        {
            this.Notifier = notifier;
            this.userProfileClient = userProfileClient;
        }

        public async Task HandleEvent(JObject eventObject)
        {
            var eventType = eventObject.Value<OccuredEventType>("EventType");
            var userIds = GetCurrentUserIds(eventObject);
            switch (eventType)
            {
                case OccuredEventType.EventoCreated:
                    var notficationData = CreateMessage(eventObject, PushMessageType.EventInvite);
                    userIds.ToList().Remove(notficationData.InitiatorId);
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.EventoUpdated:
                    await NotifyEventAndParticipantListUpdated(eventObject, userIds);
                    break;

                case OccuredEventType.EventoDeleted:
                     notficationData = CreateMessage(eventObject, PushMessageType.EventDeleted);
                    userIds.ToList().Remove(notficationData.InitiatorId);
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.EventoEnded:

                    notficationData = CreateMessage(eventObject, PushMessageType.EventEnd);
                    userIds.ToList().Remove(notficationData.InitiatorId);
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.EventoExtended:

                    notficationData = CreateMessage(eventObject, PushMessageType.EventExtend);
                    notficationData.ExtendDuration = eventObject.Value<int>("ExtendDuration");
                    userIds.ToList().Remove(notficationData.InitiatorId);
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
                    userIds = GetCurrentUserIds(eventObject);
                    userIds.ToList().Remove(notficationData.InitiatorId);
                    await NotifyEvent(notficationData, userIds);
                    break;

                case OccuredEventType.ParticipantsListUpdated:
                    await NotifyEventAndParticipantListUpdated(eventObject, userIds);
                    break;

                case OccuredEventType.ParticipantStateUpdated:
                    notficationData = CreateMessage(eventObject, PushMessageType.EventLeave);
                    notficationData.ResponderId = eventObject.Value<Guid>("RequesterId");
                    notficationData.ResponderName = eventObject.Value<string>("RequesterName");
                    notficationData.EventAcceptanceState = eventObject.Value<EventAcceptanceState>("EventAcceptanceState");
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

        private IEnumerable<Guid> GetCurrentUserIds(JObject eventObject)
        {
            return eventObject.Value<IEnumerable<Guid>>("UserIds");
        }

        private IEnumerable<Guid>GetAddedUserIds(JObject eventObject)
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
        {
            var gcmClientIds = await userProfileClient.GetGCMClientIdsByUserIds(userIds);

            this.Notifier.Notify(gcmClientIds, notificationData);
        }        

        private void NotifyAdditionalRegisteredUserInfoToHost() { }
    }
}
