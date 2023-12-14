using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Entities;

public class RoomProfile
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }  // RoomProfileId => creating table "
    public string RoomName { get; set; } 
    public float IdealTemperature { get; set; }
    public float IdealHumidity { get; set; }
    public bool IsDefault { get; set; } = false;
   
    
    public ThresholdLimits Threshold { get; set; }

    // Add other attributes specific to room profiles

    [System.Text.Json.Serialization.JsonIgnore] 
    public ICollection<SensorData>? Measurements { get; set; } = new List<SensorData>();
    
    

    public RoomProfile()
    {
    }
    
    
    [System.Text.Json.Serialization.JsonConstructor]
    public RoomProfile(long id, string roomName, float idealTemperature, float idealHumidity, [JsonProperty("threshold")] ThresholdLimits threshold)
    {
        Id = id;
        RoomName = roomName;
        IdealTemperature = idealTemperature;
        IdealHumidity = idealHumidity;
        Threshold = threshold;
    }
}
    
  
