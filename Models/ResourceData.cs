namespace SystemMonitor.Models;

public class ResourceData
{
    public double CpuUsage { get; set; }

    public double RamUsedMB { get; set; }

    public double RamTotalMB { get; set; }

    public double DiskUsedGB { get; set; }

    public double DiskTotalGB { get; set; }

    public DateTime Timestamp { get; set; }
}