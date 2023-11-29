using System.Net;
using System.Net.Sockets;
using System.Text;
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
        _tcpListener = new TcpListener(_ipAddress, Port);
        _serviceProvider = serviceProvider;
    }

    public async Task StartListeningAsync()
    {
        _tcpListener.Start();
        Console.WriteLine("TCP server started. Listening for incoming connections...");
        
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
                    
                    
                    Console.WriteLine("This should be the Temperature Integer: " + upLinkDto.temperature_integer);
                    
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