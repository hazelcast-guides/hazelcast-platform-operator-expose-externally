using Hazelcast;

class csharp_example
{
    static async Task Main(string[] args)
    {
        IHazelcastClient client;
        Action<HazelcastOptions> configureOptions = options =>
        {
            options.Networking.Addresses.Add("<EXTERNAL-IP>");
            options.Networking.ConnectionRetry.ClusterConnectionTimeoutMilliseconds = 1000;
            options.Networking.UsePublicAddresses = true;
        };

        var options = new HazelcastOptionsBuilder().With(configureOptions).Build();
        
        try 
        {
            client = await HazelcastClientFactory.StartNewClientAsync(options);
        }
        catch (Exception e) 
        {
            Console.WriteLine("Failed to connect: " + e.Message);
            return;
        }


        Console.WriteLine("Successful connection!");
        Console.WriteLine("Starting to fill the map with random entries.");

        var map = await client.GetMapAsync<string, string>("map");
        Random random = new Random();
        while (true)
        {
            int randomKey = random.Next(100_000);
            await map.PutAsync("key-" + randomKey, "value-" + randomKey);
            Console.WriteLine("Current map size: " + await map.GetSizeAsync());
        }
    }
}
