namespace TcpListenerBackgroundService.DTOs;

public class UplinkDTO
{
    public string device_UID { get; set; } 
    public int temperature_integer { get; set; }
    public int temperature_decimal { get; set; }
    public int humidity_percentage { get; set; }
}