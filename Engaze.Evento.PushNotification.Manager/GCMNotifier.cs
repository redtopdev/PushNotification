
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PushSharp.Common;
using PushSharp.Google;

using System;
using System.Collections.Generic;

namespace Engaze.Evento.PushNotification.Manager
{
    public class GCMNotifier : IPushNotifier
    {
        private IConfiguration configuration;
        private GcmServiceBroker brokerService;

        public GCMNotifier(IConfiguration config)
        {
            this.configuration = config;
            InitializeNotifier();

        }

        public void Notify<T>(List<string> registrationIds, T notificationData) where T : class
        {
            brokerService.Start();
            foreach (var regId in registrationIds)
            {
                // Queue a notification to send
                brokerService.QueueNotification(new GcmNotification
                {
                    RegistrationIds = new List<string> { regId },
                    Data = (JObject)JToken.FromObject(notificationData)
                });
            }

            brokerService.Stop();
        }

        private void InitializeNotifier()
        {
            brokerService = new GcmServiceBroker(new GcmConfiguration(configuration.GetValue<string>("AuthToken")));
            //Wire up the events for all the services that the broker registers
            brokerService.OnNotificationSucceeded += NotificationSucceeded;
            brokerService.OnNotificationFailed += NotificationFailed;
        }

        private void NotificationSucceeded(GcmNotification notification)
        {
            //throw new NotImplementedException();
        }

        private void NotificationFailed(GcmNotification notification, AggregateException exception)
        {
            exception.Handle(ex => {

                // See what kind of exception it was to further diagnose
                if (ex is GcmNotificationException)
                {
                    var x = ex as GcmNotificationException;

                    // Deal with the failed notification
                    GcmNotification n = x.Notification;
                    string description = x.Description;

                    Console.WriteLine($"Notification Failed: ID={n.MessageId}, Desc={description}");
                }
                else if (ex is GcmMulticastResultException)
                {

                    var x = ex as GcmMulticastResultException;

                    foreach (var succeededNotification in x.Succeeded)
                    {
                        Console.WriteLine($"Notification Failed: ID={succeededNotification.MessageId}");
                    }

                    foreach (var failedKvp in x.Failed)
                    {
                        GcmNotification n = failedKvp.Key;
                        var e = failedKvp.Value as GcmNotificationException;

                        Console.WriteLine($"Notification Failed: ID={n.MessageId}, Desc={e.Description}");
                    }

                }
                else if (ex is DeviceSubscriptionExpiredException)
                {
                    var x = (DeviceSubscriptionExpiredException)ex;

                    string oldId = x.OldSubscriptionId;
                    string newId = x.NewSubscriptionId;

                    Console.WriteLine($"Device RegistrationId Expired: {oldId}");

                    if (!string.IsNullOrEmpty(newId))
                    {
                        // If this value isn't null, our subscription changed and we should update our database
                        Console.WriteLine($"Device RegistrationId Changed To: {newId}");
                    }
                }
                else if (ex is RetryAfterException)
                {
                    var x = ex as RetryAfterException;
                    // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                    Console.WriteLine($"Rate Limited, don't send more until after {x.RetryAfterUtc}");
                }
                else
                {
                    Console.WriteLine("Notification Failed for some (Unknown Reason)");
                }

                // Mark it as handled
                return true;
            });
        }
    }
}