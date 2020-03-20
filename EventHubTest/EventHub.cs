using System;
using System.Text;
using Microsoft.ServiceBus.Messaging;

namespace EventHubTest
{
    class EventHub
    {

        private const string TransportType = ";TransportType=Amqp";
        private EventHubConfiguration _configuration;
        private static EventHubClient _eventHubClient;
        private MessagingFactory _messagingFactory;
        private EventHubConsumerGroup _eventHubConsumerGroup;
        public EventHub(EventHubConfiguration configuration)
        {
            _configuration = configuration;
            try
            {
                _messagingFactory = MessagingFactory.CreateFromConnectionString(_configuration.EventHubConnectionString + TransportType);
                _eventHubClient = _messagingFactory.CreateEventHubClient(_configuration.EventHubName);
                _eventHubConsumerGroup = _eventHubClient.GetConsumerGroup(_configuration.EventHubConsumer);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {e.Message}");
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
                Environment.Exit(-1);
            }
            
        }
        public void ShowEventHubRuntimeInformation()
        {
            try
            {
                var runtimeInformation = GetEventHubtRuntimeInformation();
                Console.WriteLine("Event Hub Runtime Information:");
                Console.WriteLine("-> Path: " + runtimeInformation.Path);
                Console.WriteLine("-> Creation Time: " + runtimeInformation.CreatedAt);
                Console.WriteLine("-> Partition Count: " + runtimeInformation.PartitionCount);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {e.Message}");
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
                Environment.Exit(-1);
            }
        }

        private EventHubRuntimeInformation GetEventHubtRuntimeInformation()
        {
            return _eventHubClient.GetRuntimeInformation();
        }

        public void SendMessage()
        {
            Console.WriteLine("Type the message to be sent");
            var message = Console.ReadLine();
            try
            {
                SendMessage(message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {e.Message}");
            }
        }
        
        private void SendMessage(string msg)
        {
            var message = $"EHProgram: {msg}";
            Console.WriteLine($"Sending message: {message}");
            _eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(message)));
            Console.WriteLine("Message Sent!");
        }

        public void ReadMessage()
        {
            int partitionCount = GetEventHubtRuntimeInformation().PartitionCount;
           
            for (int i = 0; i < partitionCount; i++)
            {

                var receive = true;
                string myOffset;
                var eventHubReceiver = _eventHubConsumerGroup.CreateReceiver(i.ToString());
                while (receive)
                {
                    try
                    {
                        EventData message = eventHubReceiver.Receive(new TimeSpan(0, 0, 5));
                        if (message != null)
                        {
                            myOffset = message.Offset;
                            string body = Encoding.UTF8.GetString(message.GetBytes());
                            Console.WriteLine(String.Format("Received message offset: {0} \nbody: {1}", myOffset, body));
                        }
                        else
                        {
                            Console.WriteLine($"Nothing remaining on partition: {i}");
                            receive = false;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{DateTime.Now} > Exception: {e.Message}");
                    }
                }

            }
        }
    }
}
