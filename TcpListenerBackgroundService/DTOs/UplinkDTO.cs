namespace TcpListenerBackgroundService.DTOs;

public class UplinkDTO
{
    public int temperature_integer { get; set; }
    public int temperature_decimal { get; set; }
    public int humidity_integer { get; set; }
    public int humidity_decimal { get; set; }
}