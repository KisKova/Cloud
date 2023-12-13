using System.Collections;
using EfcDataAccess.DAOs;
using Microsoft.EntityFrameworkCore;
using Moq;

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
    private UserDao _userDao;
    private LastMeasurementDao _lastMeasurementDao;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<SmartHomeSystemContext>()
           .UseInMemoryDatabase(databaseName: "TestDatabase")
          .Options;
        
        //var mockContext = new Mock<SmartHomeSystemContext>(options);
        var context = new SmartHomeSystemContext(options);
        _homeDao = new HomeDao(context);
        _userDao = new UserDao(context);
    }

    [Test]
    public async Task AddUser()
    {
        /*User user = new User(1, "Isti", "isti@gmail.com");
        User retrieved;

        await _userDao.RegisterUser(user);

        retrieved = await _userDao.GetUserById(1);

        Assert.Equals(user,retrieved);*/

        //Home home = new Home("address");

        //await _homeDao.AddNewHome(1, home);

        //_homeDao.

        // Act
        //var addedHome = await _homeDao.AddNewHome(user.Id, home);

        // Assert
        //Assert.NotNull(addedHome);
        //Assert.AreEqual(home.Address, addedHome.Address);
        // Add more assertions as per your logic
    }
}
