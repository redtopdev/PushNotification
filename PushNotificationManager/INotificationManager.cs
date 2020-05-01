using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace PushNotification.Manager
{
    public interface INotificationManager
    {
       
        Task HandleEvent(JObject eventObject);
    }
}