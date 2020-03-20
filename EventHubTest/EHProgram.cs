using System;
using System.Threading;

namespace EventHubTest
{
    class EHProgram
    {
        public static EventHubConfiguration eventHubConfiguration;
        public static EventHub eventHub;
        public static void Main(string[] args)
        {
            ShowWelcomeInformation();
            Console.WriteLine(Environment.NewLine);
            ShowProcessStep1();
            Console.WriteLine(Environment.NewLine);
            ShowProcessStep2();
        }
        private static void ShowWelcomeInformation()
        {
            Console.WriteLine("## Event Hub Connection Testing Program ##");
            Console.WriteLine("##########################################" + Environment.NewLine);
            Console.WriteLine("Created by: Unai Masa");
            Console.WriteLine("Last Update: 03/20/2020");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("This program was made to test the connection to an Azure Event Hub Service.");
            Console.WriteLine("It allows to:");
            Console.WriteLine("    - connect to an Event Hub");
            Console.WriteLine("    - send meesages to an Event Hub");
            Console.WriteLine("    - read messages from an Event Hub");
            Console.WriteLine("##########################################" + Environment.NewLine);
        }

        private static void ShowProcessStep1()
        {
            Console.WriteLine("STEP 1: Provide Event Hub Information");
            Console.WriteLine("##########################################" + Environment.NewLine);
            Console.WriteLine("-> Insert EventHubConnectionString");
            var connectionString = Console.ReadLine();
            Console.WriteLine(connectionString);
            Console.WriteLine("-> Insert EventHubName");
            var name = Console.ReadLine();
            Console.WriteLine(name);
            Console.WriteLine("-> Insert ConsumerName");
            var consumer = Console.ReadLine();
            Console.WriteLine(consumer);
            eventHubConfiguration = new EventHubConfiguration(connectionString, name, consumer);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Checking connection with the Event Hub ...");
            eventHub = new EventHub(eventHubConfiguration);
            eventHub.ShowEventHubRuntimeInformation();
            Console.WriteLine("##########################################" + Environment.NewLine);
        }

        private static void ShowProcessStep2()
        {
            var process = true;
            Console.WriteLine("STEP 2: Send or Read Information");
            Console.WriteLine("##########################################");
            while (process)
            {
                Console.WriteLine("-> Press the number of the action to execute");
                Console.WriteLine("-> (1)Send, (2)Read, (3)Exit");
                var action = Console.ReadLine();
                ProcessAction(Int32.Parse(action));
            }
            Console.WriteLine("##########################################" + Environment.NewLine);
        }

        private static void ProcessAction(int action)
        {
            switch (action)
            {
                case 1:
                    eventHub.SendMessage();
                    break;
                case 2:
                    eventHub.ReadMessage();
                    break;
                case 3:
                    ShowExitInformation();
                    Environment.Exit(-1);
                    break;
                default:
                    Console.WriteLine("Please, press one of the available options, (1)Send, (2)Read, (3)Exit.");
                    var act = Console.ReadLine();
                    ProcessAction(Int32.Parse(act));
                    break;
            }
        }

        private static void ShowExitInformation()
        {
            Console.WriteLine("#########····· SEE YOU SOON ·····#########");
            Console.WriteLine("##########################################");
            Thread.Sleep(2000);
        }
    }
}
