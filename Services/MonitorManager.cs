using SystemMonitor.Interfaces;
using Microsoft.Extensions.Options;
using SystemMonitor.Configuration;

namespace SystemMonitor.Services;

public class MonitorManager
{
    private readonly ISystemMonitor _monitor;
    private readonly IEnumerable<IMonitorPlugin> _plugins;

    private readonly MonitoringSettings _settings;

    public MonitorManager(
        ISystemMonitor monitor,
        IEnumerable<IMonitorPlugin> plugins,
        IOptions<MonitoringSettings> options)
    {
        _monitor = monitor;
        _plugins = plugins;
        _settings = options.Value;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            try
            {
                Console.Clear();

                var data = await _monitor.GetSystemDataAsync();

                Console.WriteLine("=================================");
                Console.WriteLine("       SYSTEM MONITOR");
                Console.WriteLine("=================================");

                Console.WriteLine($"CPU Usage : {data.CpuUsage}%");
                Console.WriteLine($"RAM Usage : {data.RamUsedMB} MB / {data.RamTotalMB} MB");
                Console.WriteLine($"Disk Usage: {data.DiskUsedGB} GB / {data.DiskTotalGB} GB");
                Console.WriteLine($"Time      : {data.Timestamp}");

                foreach (var plugin in _plugins)
                {
                    await plugin.ExecuteAsync(data);
                }

                await Task.Delay(_settings.IntervalSeconds * 1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}