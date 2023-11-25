using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Entities;

public class Home
{
    //[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HomeId { get; set; } // Primary key
    public string Address { get; set; }
    [JsonIgnore]
    public long UserId { get; set; }
    
    [JsonIgnore] public RoomProfile? CurrentRoomProfile { get; set; }
    
    [JsonIgnore]
    public List<RoomProfile> RoomProfiles { get; set; } = new List<RoomProfile>();
    [JsonIgnore] 
    public List<SensorData>? Measurements { get; set; } = new List<SensorData>();
  
    public string? DeviceEui { get; set; }  // need to change this method name
    
    [JsonConstructor]
    public Home(string address)
    {
        Address = address;
    }
}  
