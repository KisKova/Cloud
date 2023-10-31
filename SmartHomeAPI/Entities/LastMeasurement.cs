namespace Entities
{
    public class LastMeasurement
    {
        
        public long LastMeasurementId { get; set; } // Primary key
        public string? Name { get; set; }
        public SensorData? LastDataMeasurement { get; set; }
        public string? DeviceEui { get; set; }

        public LastMeasurement(long lastMeasurementId, string? name, SensorData? lastDataMeasurement, string? deviceEui)
        {
            LastMeasurementId = lastMeasurementId;
            Name = name;
            LastDataMeasurement = lastDataMeasurement;
            DeviceEui = deviceEui;
        }
    }
}