/// <summary>
/// Developer: ShyamSk
/// </summary>

namespace Notification.Service
{
    using Engaze.Core.Web;
    using FirebaseAdmin;
    using Google.Apis.Auth.OAuth2;

    public class Program
    {
        public static void Main(string[] args)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("FirebaseMessageCred.json"),
            });
            EngazeWebHost.Run<Startup>(args);
        }       
    }
}
