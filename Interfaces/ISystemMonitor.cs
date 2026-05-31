using SystemMonitor.Models;

namespace SystemMonitor.Interfaces;

public interface ISystemMonitor
{
    Task<ResourceData> GetSystemDataAsync();
}