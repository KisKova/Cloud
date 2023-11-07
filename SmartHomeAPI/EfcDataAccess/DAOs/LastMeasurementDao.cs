using Contracts;
using Entities;

namespace EfcDataAccess.DAOs; 

public class LastMeasurementDao : ISensorDataService {

    private readonly SmartHomeSystemContext _smartHomeSystemContext;

    public LastMeasurementDao(SmartHomeSystemContext smartHomeSystemContext) {
        _smartHomeSystemContext = smartHomeSystemContext;
    }


    public Task<ICollection<SensorData>> GetRecentMeasurements(long homeId, int amount) {
        throw new NotImplementedException();
    }

    public Task<SensorData> GetLastMeasurement(long gId) {
        throw new NotImplementedException();
    }

    public Task<ICollection<SensorData>> GetHourlyMeasurements(long homeId, int hours) {
        throw new NotImplementedException();
    }

    public Task<ICollection<SensorData>> GetDailyMeasurements(long homeId, int days) {
        throw new NotImplementedException();
    }

    public Task<ICollection<SensorData>> GetMonthlyMeasurements(long homeId, int month, int year) {
        throw new NotImplementedException();
    }

    public Task<ICollection<SensorData>> GetYearlyMeasurements(long homeId, int year) {
        throw new NotImplementedException();
    }

    public Task AddSensorMeasurement(SensorData sensorData, long homeId) {
        throw new NotImplementedException();
    }

    public Task AddSensorMeasurementWithEui(SensorData sensorData, string EUI) {
        throw new NotImplementedException();
    }
}