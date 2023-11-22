
using Contracts;
using EfcDataAccess;
using EfcDataAccess.DAOs;
using Microsoft.EntityFrameworkCore;

//WebAppbuilder initializer..

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//here add the builder.Services.AddScoped


builder.Services.AddScoped<SmartHomeSystemContext>();

builder.Services.AddScoped<IUserService, UserDao>();
builder.Services.AddScoped<IHomeService, HomeDao>();
builder.Services.AddScoped<IRoomService, RoomProfileDao>();
builder.Services.AddScoped<IMaxLimitService, ThresholdLimitsDao>();
builder.Services.AddScoped<ISensorDataService, LastMeasurementDao>();


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