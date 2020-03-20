namespace EventHubTest
{
    class EventHubConfiguration
    {
        public string EventHubConnectionString { get; }
        public string EventHubName { get; }
        public string EventHubConsumer { get; }

        public EventHubConfiguration(string connectionString, string name, string consumer)
        {
            EventHubConnectionString = connectionString;
            EventHubName = name;
            EventHubConsumer = consumer;
        }
    }
}
