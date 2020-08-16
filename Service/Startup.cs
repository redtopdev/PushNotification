/// <summary>
/// Developer: ShyamSk
/// </summary>
namespace Notification.Service
{
    using Engaze.Core.Web;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Notification.Manager;
    using Serilog;

    public class Startup : EngazeStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) { }   

        public override void ConfigureComponentServices(IServiceCollection services)
        {
            services.AddTransient<IPushNotifier, GCMNotifier>();
            services.AddTransient<IUserProfileClient, UserProfileClient>();
            services.AddTransient<INotificationManager, EventoNotificationManager>();            
        }

        public override void ConfigureComponent(IApplicationBuilder app)
        {
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
        }
    }
}
