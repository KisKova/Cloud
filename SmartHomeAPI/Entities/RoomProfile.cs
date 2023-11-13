using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Entities;

public class RoomProfile
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RoomProfileId { get; set; }
    
    public string RoomName { get; set; }
    
    public float IdealTemperature { get; set; }
    public bool IsDefault { get; set; }
    public float IdealHumidity { get; set; }

    // Add other attributes specific to room profiles

    [JsonIgnore] public List<SensorData>? Measurements { get; set; } = new List<SensorData>();

    public ThresholdLimits Limits { get; set; }

    public RoomProfile()
    {
    }
    [JsonConstructor]
    public RoomProfile(long roomProfileId, string roomName, float idealTemperature, float idealHumidity, ThresholdLimits limits)
    {
        RoomProfileId = roomProfileId;
        RoomName = roomName;
        IdealTemperature = idealTemperature;
        IdealHumidity = idealHumidity;
        Limits = limits;
    }
}
    
  
