namespace Engaze.Evento.PushNotification.Service
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using Engaze.Core.MessageBroker.Consumer;
    using Engaze.Core.Persistance.Cassandra;

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
                 services.ConfigureCassandraServices(hostContext.Configuration);
                 services.ConfigureConsumerService(hostContext.Configuration, typeof(EventoMessageHadler));
                 services.AddHostedService<EventoConsumer>();

             })
             .RunConsoleAsync();

            Console.WriteLine(ServiceName + " is started");
            Console.ReadLine();
        }
    }
}


