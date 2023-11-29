using System.Net;
using System.Net.Sockets;
using System.Text;
using Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundListenerTCP;

    public class TcpBackgroundListener
    {
        private TcpListener _tcpListener;
        private IServiceProvider _serviceProvider; // Add the service provider field

        private const int Port = 5000; // Change this to your desired port number

        public TcpBackgroundListener(IServiceProvider serviceProvider)
        {
            _tcpListener = new TcpListener(IPAddress.Any, Port);
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
                    TcpClient client = await _tcpListener.AcceptTcpClientAsync();
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
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Necessary services:
                    var homeService = scope.ServiceProvider.GetRequiredService<IHomeService>();
                    var homeLastDataService = scope.ServiceProvider.GetRequiredService<IHomeLastDataService>();
                    var maxLimitService = scope.ServiceProvider.GetRequiredService<IMaxLimitService>();
                    var roomService = scope.ServiceProvider.GetRequiredService<IRoomService>();
                    var sensorDataService = scope.ServiceProvider.GetRequiredService<ISensorDataService>();
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    
                    
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
                            // Process received data similar to your WebSocket implementation
                            string receivedData = data.ToString();
                            
                            // Your logic here...
                            
                            // Send response (if needed)
                            string response = "Response to client";
                            byte[] responseData = Encoding.UTF8.GetBytes(response);
                            await stream.WriteAsync(responseData, 0, responseData.Length);
                            
                            data.Clear();
                        }
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while processing client data: {ex.Message}");
            }
        }
    }

