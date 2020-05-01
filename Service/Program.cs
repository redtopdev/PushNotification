/// <summary>
/// Developer: ShyamSk
/// </summary>

namespace Notification.Service
{
    using Engaze.Core.Web;


    public class Program
    {
        public static void Main(string[] args)
        {
            EngazeWebHost.Run<Startup>(args);
        }       
    }
}
