using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using SystemMonitor.Configuration;
using SystemMonitor.Interfaces;
using SystemMonitor.Models;

namespace SystemMonitor.Plugins;

public class ApiPlugin : IMonitorPlugin
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _settings;

    public ApiPlugin(
        HttpClient httpClient,
        IOptions<ApiSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task ExecuteAsync(ResourceData data)
    {
        try
        {
            var payload = new
            {
                cpu = data.CpuUsage,
                ram_used = data.RamUsedMB,
                disk_used = data.DiskUsedGB
            };

            var response =
                await _httpClient.PostAsJsonAsync(
                    _settings.BaseUrl,
                    payload);

            Console.WriteLine(
                $"API Status: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"API Error: {ex.Message}");
        }
    }
}