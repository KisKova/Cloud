using System.Net;
using System.Net.Sockets;
using System.Text;
using Contracts;
using Entities;
using Newtonsoft.Json;
using TcpListenerBackgroundService.DTOs;

namespace TcpListenerBackgroundService;

public class TcpBackgroundListener
{
    private TcpListener _tcpListener;
    private readonly IServiceProvider _serviceProvider;

    // Set the IP address and port number for the server
    private const int Port = 5000;
    private readonly IPAddress _ipAddress = IPAddress.Parse("192.168.0.193");

    public TcpBackgroundListener(IServiceProvider serviceProvider)
    {
        Console.WriteLine("TCP Const created");
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
            using var scope = _serviceProvider.CreateScope();

            //var sensorDataService = _serviceProvider.GetRequiredService<ISensorDataService>();
            
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];
            StringBuilder data = new StringBuilder();

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
                    
                    Console.WriteLine("This should be the Temperature Integer: " + upLinkDto.temperature_integer);

                    //var sensorData = new SensorData(0, 25, 40);
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

                    //await sensorDataService.AddSensorMeasurement(sensorData, 1);
                    // Our logic here...
                            
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
}