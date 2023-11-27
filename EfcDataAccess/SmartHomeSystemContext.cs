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
    
    protected readonly IConfiguration Configuration;

    public SmartHomeSystemContext() {
        
    }
    
    public SmartHomeSystemContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    {
        optionsBuilder.UseNpgsql("Host=cornelius.db.elephantsql.com;Database=tckwnbjh;Port=5432;Username=tckwnbjh;Password=AVp0qq2Ls5Go3sZD4Xv3Jp97TAqu_gGv;",
                options => options.UseAdminDatabase("tckwnbjh"));
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