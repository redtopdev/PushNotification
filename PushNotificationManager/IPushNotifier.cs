using System.Collections.Generic;

namespace PushNotification.Manager
{
    public interface IPushNotifier
    {
        void Notify<T>(IEnumerable<string> registrationIds, T notificationData) where T : class;
    }
}
