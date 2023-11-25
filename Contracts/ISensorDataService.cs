using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Contracts
{
    public interface ISensorDataService
    {
        public Task<ICollection<SensorData>> GetRecentMeasurements(long homeId, int amount);
        public Task<SensorData> GetLastMeasurement(long gId);
        public Task<ICollection<SensorData>> GetHourlyMeasurements(long homeId, int hours);
        public Task<ICollection<SensorData>> GetDailyMeasurements(long homeId, int days);
        public Task<ICollection<SensorData>> GetMonthlyMeasurements(long homeId, int month, int year);
        public Task<ICollection<SensorData>> GetYearlyMeasurements(long homeId, int year);
        public Task AddSensorMeasurement(SensorData sensorData, long homeId);
        public Task AddSensorMeasurementWithEui(SensorData sensorData, string EUI);
    }
}