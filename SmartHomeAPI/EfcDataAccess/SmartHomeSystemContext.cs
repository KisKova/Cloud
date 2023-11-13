using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EfcDataAccess;

public class SmartHomeSystemContext : DbContext {
    
    public DbSet<SensorData> DataMeasures { get; set; }
    public DbSet<Home> Homes { get; set; }
    public DbSet<RoomProfile> RoomProfiles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ThresholdLimits> ThresholdsLimits { get; set; }

    //protected readonly IConfiguration _configuration;
    
    /*public SmartHomeSystemContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }*/

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = ../EfcDataAccess/SmartHome.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LastMeasurement>().HasKey(m => m.LastMeasurementId);
        
        modelBuilder.Entity<Home>().HasKey(g => g.HomeId);
        modelBuilder.Entity<RoomProfile>().HasKey(p => p.RoomProfileId);
        modelBuilder.Entity<Home>().HasIndex(g => g.DeviceEui).IsUnique();

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        
        modelBuilder.Entity<ThresholdLimits>().HasKey(t => t.Id);
    }
}