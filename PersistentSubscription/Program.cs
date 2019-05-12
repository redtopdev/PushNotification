namespace Engaze.Evento.PushNotification.Service
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using Engaze.Core.MessageBroker.Consumer;
    using Engaze.Core.Persistance.Cassandra;
    using Engaze.Evento.PushNotification.Persistance;
    using Engaze.Evento.PushNotification.Manager;

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
                 var sp = services.BuildServiceProvider();
                 CassandraRepository repo = new CassandraRepository(sp.GetService<CassandraSessionCacheManager>(), sp.GetService<CassandraConfiguration>());
                 services.AddSingleton(typeof(IDataRepository), repo);
                 services.ConfigureConsumerService(hostContext.Configuration, new EventoMessageHadler(repo,
                     new EventoNotificationManager(new GCMNotifier(hostContext.Configuration))));
                 services.AddHostedService<EventoConsumer>();

             })
             .RunConsoleAsync();

            Console.WriteLine(ServiceName + " is started");
            Console.ReadLine();
        }
    }
}


