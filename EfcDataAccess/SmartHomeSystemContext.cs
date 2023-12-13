using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EfcDataAccess; 

public class SmartHomeSystemContext : DbContext {
    
    public DbSet<SensorData> DataMeasures { get; set; } = null!;
    public DbSet<Home> Homes { get; set; } = null!;
    public DbSet<RoomProfile> RoomProfiles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ThresholdLimits> ThresholdsLimits { get; set; } = null!;
    
    protected readonly IConfiguration Configuration;

    public SmartHomeSystemContext() {
        
    }

    public SmartHomeSystemContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public SmartHomeSystemContext(DbContextOptions options):base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    {
        optionsBuilder.UseNpgsql("Host=34.79.231.88;Database=postgres;Port=5432;Username=postgres;Password=YUPn5+J)$}yMzB{-;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LastMeasurement>().HasKey(m => m.LastMeasurementId);
        
        modelBuilder.Entity<Home>().HasKey(g => g.HomeId);
        modelBuilder.Entity<RoomProfile>().HasKey(p => p.Id);
        modelBuilder.Entity<Home>().HasIndex(g => g.DeviceEui).IsUnique();

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        
        modelBuilder.Entity<ThresholdLimits>().HasKey(t => t.Id);
    }
}