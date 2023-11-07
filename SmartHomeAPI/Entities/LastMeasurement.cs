namespace Entities
{
    public class LastMeasurement
    {
        
        public int LastMeasurementId { get; set; } // Primary key
        public string? Name { get; set; }
        public SensorData? LastDataMeasurement { get; set; }
        public string? DeviceEui { get; set; }

        public LastMeasurement(int lastMeasurementId, string? name, SensorData? lastDataMeasurement, string? deviceEui)
        {
            LastMeasurementId = lastMeasurementId;
            Name = name;
            LastDataMeasurement = lastDataMeasurement;
            DeviceEui = deviceEui;
        }
        
        private LastMeasurement(){}
    }
}