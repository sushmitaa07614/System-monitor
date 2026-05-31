using SystemMonitor.Models;

namespace SystemMonitor.Interfaces;

public interface IMonitorPlugin
{
    Task ExecuteAsync(ResourceData data);
}