using System.Diagnostics;
using System.Management;
using SystemMonitor.Interfaces;
using SystemMonitor.Models;

namespace SystemMonitor.Services;

public class WindowsSystemMonitor : ISystemMonitor
{
    private readonly PerformanceCounter _cpuCounter;

    public WindowsSystemMonitor()
    {
        _cpuCounter = new PerformanceCounter(
            "Processor",
            "% Processor Time",
            "_Total");

        // Warm up the counter
        _cpuCounter.NextValue();
    }

    public async Task<ResourceData> GetSystemDataAsync()
    {
        // CPU counter needs a small delay
        await Task.Delay(1000);

        double cpuUsage = Math.Round(
            _cpuCounter.NextValue(),
            2);

        var memoryInfo = GetMemoryInfo();

        double totalRamMB = memoryInfo.TotalRamMB;
        double freeRamMB = memoryInfo.FreeRamMB;
        double usedRamMB = totalRamMB - freeRamMB;

        var drive = DriveInfo.GetDrives()
            .FirstOrDefault(d => d.IsReady &&
                                 d.DriveType == DriveType.Fixed);

        double totalDiskGB = 0;
        double usedDiskGB = 0;

        if (drive != null)
        {
            totalDiskGB =
                drive.TotalSize /
                1024.0 / 1024 / 1024;

            usedDiskGB =
                (drive.TotalSize -
                 drive.AvailableFreeSpace) /
                1024.0 / 1024 / 1024;
        }

        return new ResourceData
        {
            CpuUsage = cpuUsage,

            RamUsedMB = Math.Round(
                usedRamMB,
                2),

            RamTotalMB = Math.Round(
                totalRamMB,
                2),

            DiskUsedGB = Math.Round(
                usedDiskGB,
                2),

            DiskTotalGB = Math.Round(
                totalDiskGB,
                2),

            Timestamp = DateTime.Now
        };
    }

    private (double TotalRamMB, double FreeRamMB)
        GetMemoryInfo()
    {
        var searcher =
            new ManagementObjectSearcher(
                "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

        foreach (ManagementObject obj in searcher.Get())
        {
            double totalRamMB =
                Convert.ToDouble(
                    obj["TotalVisibleMemorySize"]) / 1024;

            double freeRamMB =
                Convert.ToDouble(
                    obj["FreePhysicalMemory"]) / 1024;

            return (
                totalRamMB,
                freeRamMB
            );
        }

        return (0, 0);
    }
}