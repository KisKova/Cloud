using System.Collections;
using EfcDataAccess.DAOs;
using Microsoft.EntityFrameworkCore;

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
    private SmartHomeSystemContext _dbContext;
    private HomeDao _homeDao;
    private LastMeasurementDao _lastMeasurementDao;

    [SetUp]
    public void Setup()
    {
        //var options = new DbContextOptionsBuilder<SmartHomeSystemContext>()
           // .UseInMemoryDatabase(databaseName: "TestDatabase")
          //  .Options;

        //_dbContext = new SmartHomeSystemContext(options);
        _homeDao = new HomeDao(_dbContext);
        _lastMeasurementDao = new LastMeasurementDao(_dbContext);
    }

    [Test]
    public async Task AddDataToDataMeasurements()
    {
        //var sensorData = new SensorData(100, 99, "SHSDIG9999");

        //sensorData.HomeId = 1;
       ;

        Console.WriteLine(_homeDao.RetrieveAllHomeIdsFromDB());

        // Act
        //var addedHome = await _homeDao.AddNewHome(user.Id, home);

        // Assert
        //Assert.NotNull(addedHome);
        //Assert.AreEqual(home.Address, addedHome.Address);
        // Add more assertions as per your logic
    }
}
