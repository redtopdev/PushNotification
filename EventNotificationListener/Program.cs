namespace PushNotification.Service
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using Engaze.Core.MessageBroker.Consumer;
    using PushNotification.Manager;

    class Program
    {
        private static string ServiceName = "Push Notification Service";
        public static void Main(string[] args)
        {
            Console.WriteLine(ServiceName + " is starting..");
            new HostBuilder().ConfigureHostConfiguration(configHost =>
            {
                configHost.AddCommandLine(args);
                configHost.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureAppConfiguration((hostContext, configApp) =>
            {
                hostContext.HostingEnvironment.EnvironmentName = System.Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            })
             .ConfigureLogging((hostContext, configLogging) =>
             {
                 if (hostContext.HostingEnvironment.IsDevelopment())
                 {
                     configLogging.AddConsole();
                     configLogging.AddDebug();
                 }

             }).ConfigureServices((hostContext, services) =>
             {
                 services.AddLogging();
               
                 var sp = services.BuildServiceProvider();                
                 services.ConfigureConsumerService(hostContext.Configuration, 
                     new EventoMessageHadler(
                         new EventoNotificationManager(new GCMNotifier(hostContext.Configuration), 
                         new UserProfileClient(hostContext.Configuration))));
                 services.AddHostedService<EventoConsumer>();

             })
             .RunConsoleAsync();

            Console.WriteLine(ServiceName + " is started");
            Console.ReadLine();
        }
    }
}


