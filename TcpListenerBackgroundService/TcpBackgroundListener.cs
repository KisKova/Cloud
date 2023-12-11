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
            Console.WriteLine("Now we're setting the necessary services!");

            var sensorDataService = _serviceProvider.GetRequiredService<ISensorDataService>();
            var homeService = _serviceProvider.GetRequiredService<IHomeService>();
            var limitService = _serviceProvider.GetRequiredService<IMaxLimitService>();
            var notificationService = _serviceProvider.GetRequiredService<INotificationSender>();
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];
            StringBuilder data = new StringBuilder();
            
            Console.WriteLine("Start listening...");

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead <= 0) break;

                data.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                if (!stream.DataAvailable)
                {
                    string receivedData = data.ToString();
                    
                    var upLinkDto = JsonConvert.DeserializeObject<UplinkDTO>(receivedData);
                    
                    Console.WriteLine("Received data: " + receivedData);
                    
                    ArrayList homeIdsList = await homeService.RetrieveAllHomeIdsFromDB();
                    
                    // Visualizing elements of the ArrayList and sending data to the database
                    foreach (long id in homeIdsList)
                    {
                        Console.WriteLine("HomeId: " + id); // Print each ID to the console
                        
                        SensorData sensorData = new SensorData();
                        
                        sensorData.HumidityData = upLinkDto.humidity_percentage;
                        sensorData.TemperatureData = ConstructTemperature(upLinkDto.temperature_integer, upLinkDto.temperature_decimal);
                        sensorData.HomeId = id;
                        //sensorData.Timestamp = DateTime.Now;
                        sensorData.DeviceEui = upLinkDto.device_UID;
                        
                        Console.WriteLine("Adding the data to the database...");
                        await sensorDataService.AddSensorMeasurement(sensorData,id);
                        
                        Console.WriteLine("Check the threshold limits...");
                        var limits = await limitService.RetrieveThresholdForCurrentRoom(id);
                        if (limits != null)
                        {
                            if (sensorData.TemperatureData < limits.TemperatureMin ||
                                sensorData.HumidityData < limits.HumidityMin)
                            {
                                var message = "The temperature/humidity is too high. Window is opening...";
                                Console.WriteLine(message);

                                await SendDataToIoT("ID",0, stream);

                                long userId = await homeService.RetrieveUserIdByHomeId(id);

                                Console.WriteLine("UserId: " + userId);

                                var token = await userService.GetUserTokenById(userId);

                                Console.WriteLine("UserToken: " + token);

                                if (token != null)
                                {
                                    await notificationService.SendUserNotificationAsync(token, 
                                        "High temperature/humidity! Limit has been reached!",
                                        message);
                                }
                            }
                            else if (sensorData.TemperatureData > limits.TemperatureMax ||
                                     sensorData.HumidityData > limits.HumidityMax)
                            {
                                var message = "The temperature/humidity is too low. Window is closing...";
                                Console.WriteLine(message);

                                await SendDataToIoT("ID",90, stream);

                                long userId = await homeService.RetrieveUserIdByHomeId(id);

                                Console.WriteLine("UserId: " + userId);

                                var token = await userService.GetUserTokenById(userId);

                                Console.WriteLine("UserToken: " + token);

                                if (token != null)
                                {
                                    await notificationService.SendUserNotificationAsync(token, 
                                        "Low temperature/humidity! Limit has been reached!",
                                        message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Temperature and humidity are inside the ideal range!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("There are no limits.");
                        }
                    }
                            
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

    private async Task SendDataToIoT(string deviceId, int servoAngel, NetworkStream str)
    {
        DownlinkDTO downLinkDto = new ()
        {
            device_UID = deviceId,
            servo_normal = servoAngel
        };
                    
        //serialize to json
        var downLinkJson = JsonConvert.SerializeObject(downLinkDto);
        //send serialized DownLink
        await str.WriteAsync(Encoding.UTF8.GetBytes(downLinkJson), 0, Encoding.UTF8.GetBytes(downLinkJson).Length);
        Console.WriteLine("Data sent to client.");
    }

    private float ConstructTemperature(int whole, int dec)
    {
        float rest = dec;
        float temp = whole + rest / 10;
        return temp;
    }
}