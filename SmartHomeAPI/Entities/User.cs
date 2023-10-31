using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    [JsonIgnore]
    public string? Token { get; set; }
    public string? Email { get; set; }
    [JsonIgnore]
    public ICollection<Home>? Homes { get; set; } = new List<Home>();

    [JsonIgnore]
    public ICollection<RoomProfile>? RoomProfiles { get; set; } = new List<RoomProfile>();

    public User()
    {
    }

    [JsonConstructor]
    public User(long id, string username, string email)
    {
        Id = id;
        Username = username;
        Email = email;
    }  
}