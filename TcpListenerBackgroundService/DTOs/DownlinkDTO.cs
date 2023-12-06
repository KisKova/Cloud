namespace TcpListenerBackgroundService.DTOs;

public class DownlinkDTO
{
    public int temperature_limit_high { get; set; }
    public int temperature_limit_low { get; set; }
    public int humidity_limit_high { get; set; }
    public int humidity_limit_low { get; set; }
    public int servo_limit_high { get; set; }
    public int servo_normal { get; set; }
    public int servo_limit_low { get; set; }
}