using System.Collections.Generic;

namespace Notification.Manager
{
    public interface IPushNotifier
    {
        void Notify<T>(IEnumerable<string> registrationIds, T notificationData) where T : class;
    }
}
