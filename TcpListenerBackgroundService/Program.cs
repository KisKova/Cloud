using Contracts;
using EfcDataAccess.DAOs;
using Entities;
using MyNotificationService;
using TcpListenerBackgroundService;

public class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices(); // Set up your service provider
            
        var tcpListener = new TcpBackgroundListener(serviceProvider);
        var serverTask = tcpListener.StartListeningAsync();

        Console.WriteLine("Press any key to stop the server...");
        Console.Read();

        // Optionally, you might add cancellation logic here to stop the server gracefully.
        // For simplicity, this example stops the server immediately when a key is pressed.
        serverTask.Wait(); // Wait for the server task to complete
    }

    static IServiceProvider ConfigureServices()
    {
        // Set up your service collection and register services here
        var serviceCollection = new ServiceCollection();
        // Add services if needed using serviceCollection.AddTransient/Scoped/Singleton
        serviceCollection.AddScoped<ISensorDataService, LastMeasurementDao>();
        serviceCollection.AddScoped<EfcDataAccess.SmartHomeSystemContext>();
        serviceCollection.AddScoped<IHomeService, HomeDao>();
        serviceCollection.AddScoped<IRoomService, RoomProfileDao>();
        serviceCollection.AddScoped<IMaxLimitService, ThresholdLimitsDao>();
        serviceCollection.AddScoped<INotificationSender, NotificationSender>();
        serviceCollection.AddScoped<IUserService, UserDao>();
        
        // Build the service provider
        return serviceCollection.BuildServiceProvider();
    }
}