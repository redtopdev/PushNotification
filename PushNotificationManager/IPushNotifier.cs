using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.Manager
{
    public interface IPushNotifier
    {
        Task Notify<T>(IEnumerable<string> registrationIds, T notificationData, string notificationType = null) where T : class;
    }
}
