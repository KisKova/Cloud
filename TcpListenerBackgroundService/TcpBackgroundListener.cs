using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Contracts;
using Entities;
using MyNotificationService;
using Newtonsoft.Json;
using TcpListenerBackgroundService.DTOs;

namespace TcpListenerBackgroundService;

public class TcpBackgroundListener
{
    private readonly TcpListener _tcpListener;
    private readonly IServiceProvider _serviceProvider;

    // Set the IP address and port number for the server
    private const int Port = 5000;
    private readonly IPAddress _ipAddress = IPAddress.Parse("192.168.0.193");

    public TcpBackgroundListener(IServiceProvider serviceProvider)
    {
        Console.WriteLine("TCP from constr created");
        _tcpListener = new TcpListener(_ipAddress, Port);
        _serviceProvider = serviceProvider;
    }

    public async Task StartListeningAsync()
    {
        Console.WriteLine("TCP server started. Listening for incoming connections...");
        _tcpListener.Start();

        try
        {
            while (true)
            {
                var client = await _tcpListener.AcceptTcpClientAsync();
                _ = Task.Run(() => ProcessClientDataAsync(client));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
        }
        finally
        {
            _tcpListener.Stop();
            Console.WriteLine("TCP server stopped.");
        }
    }

    private async Task ProcessClientDataAsync(TcpClient client)
    {
        try
        {
            //using var scope = _serviceProvider.CreateScope();
            
            Console.WriteLine("Now we set the service!");

            var sensorDataService = _serviceProvider.GetRequiredService<ISensorDataService>();
            var roomService = _serviceProvider.GetRequiredService<IRoomService>();
            var homeService = _serviceProvider.GetRequiredService<IHomeService>();
            var limitService = _serviceProvider.GetRequiredService<IMaxLimitService>();
            var notificationService = _serviceProvider.GetRequiredService<INotificationSender>();
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];
            StringBuilder data = new StringBuilder();
            
            Console.WriteLine("Receiving data...");

            bool notification = false;

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead <= 0) break;

                data.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                if (!stream.DataAvailable)
                {
                    string receivedData = data.ToString();
                    
                    //var upLinkDto = JsonConvert.DeserializeObject<UplinkDTO>(receivedData); IMPORTANT
                    Console.WriteLine("Received data: " + receivedData);
                    
                    ArrayList homeIdsList = await homeService.RetrieveAllHomeIdsFromDB();
                    
                    // Visualizing elements of the ArrayList and sending data to the database
                    foreach (long id in homeIdsList)
                    {
                        Console.WriteLine("HomeId: " + id); // Print each ID to the console
                        
                        SensorData sensorData = new SensorData();
                        
                        sensorData.HumidityData = 46;
                        sensorData.TemperatureData = 26;
                        sensorData.HomeId = id;
                        sensorData.Timestamp = DateTime.Now;
                        sensorData.DeviceEui = "SSHDI7";
                        
                        Console.WriteLine("Adding the data to the database...");
                        await sensorDataService.AddSensorMeasurement(sensorData,id);
                        
                        Console.WriteLine("Check the threshold limits...");
                        var limits = await limitService.RetrieveThresholdForCurrentRoom(id);
                        if (sensorData.TemperatureData < limits.TemperatureMin || 
                            sensorData.HumidityData < limits.HumidityMin)
                        {
                            var message = "The temperature/humidity is too high. Window is opening...";
                            Console.WriteLine(message);
                            
                            await SendDataToIoT(60, stream);

                            long userId = await homeService.RetrieveUserIdByHomeId(id);
                            
                            Console.WriteLine("UserId: " + userId);

                            var token = await userService.GetUserTokenById(userId);
                            
                            Console.WriteLine("UserToken: " + token);

                            if (token != null)
                            {
                                await notificationService.SendUserNotificationAsync(token, "High temperature/humidity! Limit has been reached!",
                                                                message);
                            }
                        } else if (sensorData.TemperatureData > limits.TemperatureMax ||
                                   sensorData.HumidityData > limits.HumidityMax)
                        {
                            var message = "The temperature/humidity is too low. Window is closing...";
                            Console.WriteLine(message);
                            
                            await SendDataToIoT(0, stream);
                            
                            long userId = await homeService.RetrieveUserIdByHomeId(id);
                            
                            Console.WriteLine("UserId: " + userId);

                            var token = await userService.GetUserTokenById(userId);
                            
                            Console.WriteLine("UserToken: " + token);

                            if (token != null)
                            {
                                await notificationService.SendUserNotificationAsync(token, "Low temperature/humidity! Limit has been reached!",
                                    message);
                            }
                        }
                    }
                    
                    

                    DownlinkDTO downLinkDto = new ()
                    {
                        temperature_limit_high = 40,
                        temperature_limit_low = 0,
                        humidity_limit_high = 70,
                        humidity_limit_low = 30,
                        servo_limit_high = 180,
                        servo_normal = 72,
                        servo_limit_low = 0,
                    };
                    
                    //serialize to json
                    var downLinkJson = JsonConvert.SerializeObject(downLinkDto);
                    //send serialized DownLink
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(downLinkJson), 0, Encoding.UTF8.GetBytes(downLinkJson).Length);
                    Console.WriteLine("Data sent to client.");
                            
                    data.Clear();
                }
            }
                
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred while processing client data: {e.Message}");
            throw;
        }
    }

    private async Task SendDataToIoT(int servoAngel, NetworkStream str)
    {
        DownlinkDTO downLinkDto = new ()
        {
            temperature_limit_high = 40,
            temperature_limit_low = 0,
            humidity_limit_high = 70,
            humidity_limit_low = 30,
            servo_limit_high = 180,
            servo_normal = servoAngel,
            servo_limit_low = 0,
        };
                    
        //serialize to json
        var downLinkJson = JsonConvert.SerializeObject(downLinkDto);
        //send serialized DownLink
        await str.WriteAsync(Encoding.UTF8.GetBytes(downLinkJson), 0, Encoding.UTF8.GetBytes(downLinkJson).Length);
        Console.WriteLine("Data sent to client.");
    }
}