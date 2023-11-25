using System.Diagnostics.Metrics;
using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcDataAccess.DAOs; 

public class LastMeasurementDao : ISensorDataService {

    private readonly SmartHomeSystemContext _smartHomeSystemContext;

    public LastMeasurementDao(SmartHomeSystemContext smartHomeSystemContext) {
        _smartHomeSystemContext = smartHomeSystemContext;
    }


    public async Task<ICollection<SensorData>> GetRecentMeasurements(long homeId, int amount) {
        ICollection<SensorData> dataMeasures = await _smartHomeSystemContext.DataMeasures!.Take(amount)
                .Where(dm => dm.HomeId == homeId)
                .OrderByDescending(dm => dm.Timestamp)
                .ToListAsync();
        return dataMeasures;
    }

    public async Task<SensorData> GetLastMeasurement(long hId) {
        SensorData dataMeasures = await _smartHomeSystemContext.DataMeasures!
                .Where(dm => dm.HomeId == hId)
                .OrderByDescending(dm => dm.Timestamp)
                .FirstAsync();
        return dataMeasures;
    }

    public async Task<ICollection<SensorData>> GetHourlyMeasurements(long homeId, int hours) {

        TimeSpan timeSpan = new TimeSpan(hours, 0, 0);
        DateTime temp = DateTime.Now.Subtract(timeSpan);
        ICollection<SensorData> dataMeasures =
                await _smartHomeSystemContext.DataMeasures!
                        .Where(dm => dm.HomeId == homeId && dm.Timestamp > temp)
                        .ToListAsync();
        return dataMeasures;
    }

    public async Task<ICollection<SensorData>> GetDailyMeasurements(long homeId, int days) {

        TimeSpan timeSpan = new TimeSpan(days, 0, 0, 0);
        DateTime temp = DateTime.Now.Subtract(timeSpan);
        ICollection<SensorData> dataMeasures =
                await _smartHomeSystemContext.DataMeasures!
                        .Where(dm => dm.Timestamp > temp && dm.HomeId == homeId)
                        .ToListAsync();
        return dataMeasures;
    }

    public async Task<ICollection<SensorData>> GetMonthlyMeasurements(long homeId, int month, int year) {
       
        ICollection<SensorData> dataMeasures =
                await _smartHomeSystemContext.DataMeasures!
                        .Where(m => m.Timestamp.Month == month && m.Timestamp.Year==year && m.HomeId==homeId)
                        .ToListAsync();
        return dataMeasures;
    }

    public async Task<ICollection<SensorData>> GetYearlyMeasurements(long homeId, int year) {
        
        ICollection<SensorData> dataMeasures =
                await _smartHomeSystemContext.DataMeasures!
                        .Where(m => m.Timestamp.Year == year && m.HomeId==homeId)
                        .ToListAsync();
        return dataMeasures;    
    }

    public async Task AddSensorMeasurement(SensorData sensorData, long homeId) {

        Home home;
        try
        {
            home = await _smartHomeSystemContext.Homes!.Include(h => h.CurrentRoomProfile)
                    .Include(h => h.Measurements)
                    .FirstAsync(h => h.HomeId == homeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Home not found.");
        }
        
        home.Measurements!.Add(sensorData);
        _smartHomeSystemContext.Update(home);
        if (home.CurrentRoomProfile != null)
        {
            RoomProfile profile;
            try
            {
                profile = await _smartHomeSystemContext.RoomProfiles!.FirstAsync(p =>
                        p.RoomProfileId == home.CurrentRoomProfile!.RoomProfileId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Room profile not found.");
            }
            
            profile.Measurements!.Add(sensorData);
            _smartHomeSystemContext.Update(profile);
        }

        await _smartHomeSystemContext.DataMeasures!.AddAsync(sensorData);
        await _smartHomeSystemContext.SaveChangesAsync();
        await Console.Out.WriteLineAsync("LastMeasurementDAO: " + sensorData + "added to DB");
        
    }
    
    

    public async Task AddSensorMeasurementWithEui(SensorData sensorData, string eui) {
        ICollection<Home> homes;
        try
        {
            homes = await _smartHomeSystemContext.Homes!
                    .Include(h => h.CurrentRoomProfile)
                    .Include(h => h.Measurements)
                    .Where(h => h.DeviceEui == eui)
                    .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Homes not found.");
        }

        try
        {
            foreach (var home in homes)
            {
                SensorData sd = new SensorData(sensorData.SensorDataId, sensorData.TemperatureData, sensorData.HumidityData);
                home.Measurements!.Add(sd);
                await _smartHomeSystemContext.DataMeasures!.AddAsync(sd);
                _smartHomeSystemContext.Homes!.Update(home);
                await _smartHomeSystemContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            throw new Exception("Something went wrong when updating the database.");
            
        }

        await Console.Out.WriteLineAsync("LastMeasurementDAO: " + sensorData + " added to DB");
    }
}