using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcDataAccess.DAOs; 

public class HomeDao : IHomeService {

    private readonly SmartHomeSystemContext _smartHomeSystemContext;

    public HomeDao(SmartHomeSystemContext smartHomeSystemContext) {
        _smartHomeSystemContext = smartHomeSystemContext;
    }

    public async Task<Home> AddHome(long uid, Home home) {
        await _smartHomeSystemContext!.Homes!.AddAsync(home);
        User user;

        try
        {
            user = await _smartHomeSystemContext.Users!.FirstAsync(u => u.Id == uid);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User could not be found.");
        }

        try
        {
            user.Homes!.Add(home);
            _smartHomeSystemContext.Users!.Update(user);
            await _smartHomeSystemContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("The home could not be added." +
                                "It may be because a home with the same Device EUI already exists.");
        }

        return home;
    }

    public Task<Home> AddNewHome(long userId, Home home) {
        throw new NotImplementedException();
    }

    public Task<Home> DeleteHome(long homeId) {
        throw new NotImplementedException();
    }

    public Task<Home> ModifyHome(Home home) {
        throw new NotImplementedException();
    }

    public Task<ICollection<Home>> RetrieveUserHomes(long userId) {
        throw new NotImplementedException();
    }

    public Task<Home> GetLastMeasurementAtHome() {
        throw new NotImplementedException();
    }

    public Task<ICollection<LastMeasurement>> RetrieveHomesWithLastMeasurement(long userId) {
        throw new NotImplementedException();
    }

    public Task<long> RetrieveHomeIdByEui(string eui) {
        throw new NotImplementedException();
    }
}