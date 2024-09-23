namespace DeviceService;

public class DeviceResponse
{
    public long id { get; set; }
    public string type { get; set; }
    public string serial_number { get; set; }
    public string status { get; set; }
    public long house_id { get; set; }
    public DateTime created_at { get; set; }
}