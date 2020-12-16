using FirebaseAdmin.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Manager
{
    public class FCMNotifier : IPushNotifier
    {
        public async Task Notify<T>(IEnumerable<string> registrationIds, T notificationData, string notificationType = null) where T : class
        {
            List<string> fcmClientids = new List<string>();
            fcmClientids.Add("cFo_GGivRnWzPzTxVTEZPy:APA91bFmMOmoScO_-sUCk6TNVQ_GupCe4IoNcatgg9_Vlrzc15DoslZuyWnrJJj9Ve_W_fooRjICjGxjZLDrmOHsg6ifdU8qOpjvZHrO_DSPBoZfLlprsi-b1abwBezZ1dyf70gtnH6_");
            try
            {
                var message = new MulticastMessage()
                {
                    Tokens = fcmClientids,
                    Data = new Dictionary<string, string>()
                    {
                        {"Type", notificationType?? string.Empty},
                        {"Data",  JsonConvert.SerializeObject(notificationData)},
                    },
                };
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in NotifyEvent. " + ex.Message);
            }
        }
    }
}
