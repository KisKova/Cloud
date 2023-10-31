using System.ComponentModel.DataAnnotations.Schema;
namespace Entities;

public class SensorData
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SensorDataId { get; set; } // Primary key
    public float TemperatureData { get; set; }
    public float HumidityData { get; set; }
    
    public DateTime Timestamp { get; set; }

    public SensorData()
    {
    }

    public SensorData(int sensorDataId, float temperatureData, float humidityData)
    {
        SensorDataId = sensorDataId;
        TemperatureData = temperatureData;
        HumidityData = humidityData;
        Timestamp = DateTime.Now;
    }

    // Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(ts).DateTime;
    public override string ToString()
    {
        return $"SensorData-{SensorDataId}:\n\tTemperature: {TemperatureData}°C, Humidity: {HumidityData}% - at {Timestamp}";
    }
} 
