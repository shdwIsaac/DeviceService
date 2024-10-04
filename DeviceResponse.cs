namespace DeviceService;

public class DeviceResponse
{
    public long id { get; set; }
    public double targetTemperature { get; set; }
    public double currentTemperature { get; set; }
    public bool on { get; set; }
}