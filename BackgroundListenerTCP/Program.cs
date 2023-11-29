using BackgroundListenerTCP;
using Contracts;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static async Task Main()
    {
        // Create a service collection and add your services
        var services = new ServiceCollection();
        services.AddSingleton<IHomeService>();
        services.AddSingleton<IHomeLastDataService>();
        services.AddSingleton<IMaxLimitService>();
        services.AddSingleton<IRoomService>();
        services.AddSingleton<ISensorDataService>();
        services.AddSingleton<IUserService>();// Replace YourService with the actual service implementation
        // Add any other services needed by your application

        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();

        // Create an instance of TcpBackgroundListener and start listening
        var tcpListener = new TcpBackgroundListener(serviceProvider);
        await tcpListener.StartListeningAsync();

        // You might have other code or tasks running here...
        // Ensure the application doesn't terminate immediately
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}