using System.Text.Json;
using SystemMonitor.Interfaces;
using SystemMonitor.Models;

namespace SystemMonitor.Plugins;

public class FileLoggerPlugin : IMonitorPlugin
{
    private readonly string _logFilePath;

    public FileLoggerPlugin()
    {
        Directory.CreateDirectory("Logs");

        _logFilePath = Path.Combine( "Logs","system-log.txt");
    }

    public async Task ExecuteAsync(ResourceData data)
    {
        try
        {
            string logEntry =
                JsonSerializer.Serialize(data);

            await File.AppendAllTextAsync(
                _logFilePath,
                logEntry + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"File Logger Error: {ex.Message}");
        }
    }
}