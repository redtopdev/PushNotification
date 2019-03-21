﻿
using System;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace PersistentSubscription
{
    /*
    * This example sets up a volatile subscription to a test stream.
    * 
    * As written it will use the default ipaddress (loopback) and the default tcp port 1113 of the event
    * store. In order to run the application bring up the event store in another window (you can use
    * default arguments eg EventStore.ClusterNode.exe) then you can run this application with it. Once 
    * this program is running you can run the WritingEvents sample to write some events to the stream
    * and they will appear over the catch up subscription. You can also run many concurrent instances of this
    * program and each will receive the events over the subscription.
    * 
    */
    class Program
    {
      
        const int DEFAULTPORT = 1113;
        const string STREAM = "$ce-Evento";

        static void Main(string[] args)
        {

            //uncommet to enable verbose logging in client.
            var settings = ConnectionSettings.Create();//.EnableVerboseLogging().UseConsoleLogger();
            using (var conn = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, DEFAULTPORT)))
            {
                conn.ConnectAsync().Wait();
                
                
                conn.SubscribeToStreamAsync(STREAM, true, (_, x) =>
                {
                   
                    var data = Encoding.ASCII.GetString(x.Event.Data);
                    Console.WriteLine("Received: " + x.Event.EventStreamId + ":" + x.Event.EventNumber);
                    Console.WriteLine(data);
                }, null, new UserCredentials("admin", "changeit"));

              
            }
            Console.WriteLine("waiting for events. press enter to exit");
            Console.ReadLine();
        }
    }
}
