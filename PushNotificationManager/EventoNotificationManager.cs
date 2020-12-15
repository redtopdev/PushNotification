using Engaze.Core.DataContract;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;
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
            var eventObject = eventNotificationJObj.Value<JObject>("Event");
            var notficationData = CreateMessage(eventObject, PushMessageType.EventEnd);
            var userIds = GetCurrentUserIds(eventObject);
            userIds.ToList().Remove(notficationData.InitiatorId);
       
            OccuredEventType eventType = ParseMyEnum<OccuredEventType>(eventNotificationJObj.Value<string>("NotificationType"));

          
            switch (eventType)
            {
                case OccuredEventType.EventoCreated:
                     notficationData = CreateMessage(eventObject, PushMessageType.EventInvite);
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

        private IEnumerable<Guid> GetCurrentUserIds(JObject eventObject)
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
        {
            List<string> fcmClientids = new List<string>();
            fcmClientids.Add("cFo_GGivRnWzPzTxVTEZPy:APA91bFmMOmoScO_-sUCk6TNVQ_GupCe4IoNcatgg9_Vlrzc15DoslZuyWnrJJj9Ve_W_fooRjICjGxjZLDrmOHsg6ifdU8qOpjvZHrO_DSPBoZfLlprsi-b1abwBezZ1dyf70gtnH6_");
            try
            {             

                var registrationTokens = userIds;
                var message = new MulticastMessage()
                {
                    Tokens = fcmClientids,
                    Data = new Dictionary<string, string>()
                    {
                        {"EventType", notificationData.MessageType.ToString()},
                        {"Data",  JsonConvert.SerializeObject(notificationData)},
                    },
                };
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                return ;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in NotifyEvent. " + ex.Message);
            }
        }

        private void NotifyAdditionalRegisteredUserInfoToHost() { }
    }
}
