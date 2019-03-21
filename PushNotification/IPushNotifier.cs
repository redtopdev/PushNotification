using System.Collections.Generic;

namespace PushNotification
{
    public interface IPushNotifier
    {
        void Notify<T>(List<string> registrationIds, T notificationData) where T : class;
    }
}
