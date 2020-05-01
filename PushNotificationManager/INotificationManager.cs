using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Notification.Manager
{
    public interface INotificationManager
    {
       
        Task HandleEvent(JObject eventObject);
    }
}