
using Contracts;
using EfcDataAccess;
using EfcDataAccess.DAOs;
using FirebaseAdmin;
//using FirebaseNotificationClient;
using Google.Apis.Auth.OAuth2;
using Microsoft.OpenApi.Models;
using MyNotificationService;

//using WsListenerBackgroundService;
//WebAppbuilder initializer..

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Home System Data Tier API", Version = "v1" });
    
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "ApiKey must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "ApiKey",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    
    var requirement = new OpenApiSecurityRequirement
    {
        { key, new List<string>() }
    };
    
    c.AddSecurityRequirement(requirement);
});

if (FirebaseApp.DefaultInstance == null) {
    var defaultApp = FirebaseApp.Create(new AppOptions() {
        Credential = GoogleCredential
            .FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.json")),
    });

    Console.WriteLine(defaultApp.Name);
}

//here add the builder.Services.AddScoped



//adding all services
builder.Services.AddScoped<SmartHomeSystemContext>();

builder.Services.AddScoped<IUserService, UserDao>();
builder.Services.AddScoped<IHomeService, HomeDao>();
builder.Services.AddScoped<IRoomService, RoomProfileDao>();
builder.Services.AddScoped<IMaxLimitService, ThresholdLimitsDao>();
builder.Services.AddScoped<ISensorDataService, LastMeasurementDao>();
builder.Services.AddScoped<INotificationSender, NotificationSender>();


//Ws-client
//builder.Services.AddHostedService<BackgroundListener>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();