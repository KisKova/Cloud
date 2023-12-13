using System.Collections;
using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcDataAccess.DAOs; 

public class HomeDao : IHomeService {

    private readonly SmartHomeSystemContext _smartHomeSystemContext;

    public HomeDao(SmartHomeSystemContext smartHomeSystemContext) {
        _smartHomeSystemContext = smartHomeSystemContext;
    }

    public async Task<Home> AddNewHome(long uid, Home home) {
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

    public async Task<Home> DeleteHome(long homeId) {

        Home? home;
        
        try
        {
            home = await _smartHomeSystemContext.Homes!.FindAsync(homeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Home could not be found.");
        }

        Home temp = home!;

        _smartHomeSystemContext.Homes!.Remove(home!);
        await _smartHomeSystemContext.SaveChangesAsync();
        return temp;
    }

    public async Task<Home> ModifyHome(Home home) {
        try
        {
            _smartHomeSystemContext.Homes!.Update(home);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Home could not be found.");
        }

        await _smartHomeSystemContext.SaveChangesAsync();
        return home;
    }

    public async Task<ICollection<Home>> RetrieveUserHomes(long userId) {
        User user;
        try
        {
            user = await _smartHomeSystemContext.Users!.Include(u => u.Homes).FirstAsync(u => u.Id == userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return user.Homes!;
    }

    public async Task<long> RetrieveUserIdByHomeId(long homeId)
    {
        Home home;
        try
        {
            home = await _smartHomeSystemContext.Homes!.FirstAsync(h => h.HomeId == homeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Home not found.");
        }

        return home.UserId;
    }

    public async Task<Home> GetLastMeasurementAtHome() {

        SensorData sensorData;
        try
        {
            sensorData = await _smartHomeSystemContext.DataMeasures!.OrderBy(sd => sd.Timestamp).FirstAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There are no measurements available.");
        }

        Home home;

        try
        {
            home = await _smartHomeSystemContext.Homes!.FirstAsync(h => h.HomeId == sensorData.HomeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something went wrong. Either there is no data available or linked with any Home.");
        }

        return home;
    }

    public async Task<ArrayList> RetrieveAllHomeIdsFromDB() {
        ArrayList homeIds = new ArrayList();
        try
        {
            ICollection<Home> homes = await _smartHomeSystemContext.Homes!.ToListAsync();

            // Extracting homeIds from the retrieved homes
            foreach (var home in homes)
            {
                homeIds.Add(home.HomeId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred trying to retrieve all home IDs.");
            throw;
        }

        return homeIds;
    }


    //Method to get all homes from all users.*
    public async Task<ICollection<Home>> RetrieveAllHomesFromSystem() {
        ICollection<Home> homes = new List<Home>();
        try
        {
            homes = await _smartHomeSystemContext.Homes!.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An error occurred trying to retrieve all homes.");
        }

        return homes;
    }

    public async Task<ICollection<LastMeasurement>> RetrieveHomesWithLastMeasurement(long userId) {
        User user;
        ICollection<Home> homes = new List<Home>();
        ICollection<LastMeasurement> homeLastMeasurements = new List<LastMeasurement>();

        try
        {
            user = await _smartHomeSystemContext.Users!.Include(u => u.Homes).FirstAsync(u => u.Id == userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User not found.");
        }
        
        //iterating through homes and fetching measurements (Humidity, Temp)
        //creating a new List of both objects.
        homes = user.Homes!;
        foreach (Home home in homes)
        {
            Home h = await _smartHomeSystemContext.Homes!.Include(h => h.Measurements)
                    .FirstAsync(h => h.HomeId == home.HomeId);
            home.Measurements = h.Measurements;
            if (home.Measurements == null || home.Measurements!.Count == 0)
            {
                homeLastMeasurements.Add(new LastMeasurement(home.HomeId,home.Address, new SensorData(), home.DeviceEui));
            }
            else
            {
                homeLastMeasurements
                        .Add(new LastMeasurement(home.HomeId,
                                home.Address,
                                home.Measurements!
                                        .OrderBy(sd => sd.Timestamp)
                                        .First(), home.DeviceEui));
            }
        }

        return homeLastMeasurements;
    }

    public async Task<long> RetrieveHomeIdByEui(string eui) {

        Home home;

        try
        {
            home = await _smartHomeSystemContext.Homes!.FirstAsync(h => h.DeviceEui == eui);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Home could not be found.");
        }

        return home.HomeId;
    }
}