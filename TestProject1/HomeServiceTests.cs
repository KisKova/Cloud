namespace TestProject1;

using Contracts;
using EfcDataAccess;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

[TestFixture]
public class HomeServiceTests
{
    //private DbContextOptions<SmartHomeSystemContext> _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly SmartHomeSystemContext context;
            
    
    [SetUp]
    public void Setup()
    {
        var sensorDataService = _serviceProvider.GetRequiredService<ISensorDataService>();
    }

    [Test]
    public async Task AddNewHome_SuccessfullyAddsHome()
    {
        // Arrange
        var sensorDataService = _serviceProvider.GetRequiredService<ISensorDataService>();
        var userService = _serviceProvider.GetRequiredService<IUserService>();
        var homeService = _serviceProvider.GetRequiredService<IHomeService>();

        var user = new User(7, "Marcell", "309773@viauc.dk");
        
        // Act
        await userService.RegisterUser(user);

         // Ensure it's the same home that was added
        
    }
}
