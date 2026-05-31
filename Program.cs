using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemMonitor.Configuration;
using SystemMonitor.Interfaces;
using SystemMonitor.Plugins;
using SystemMonitor.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Configuration Binding
        services.Configure<ApiSettings>(
            context.Configuration.GetSection("ApiSettings"));

        services.Configure<MonitoringSettings>(
            context.Configuration.GetSection("MonitoringSettings"));

        // System Monitor Service
        services.AddSingleton<ISystemMonitor,
                              WindowsSystemMonitor>();

        // Plugins
        services.AddSingleton<IMonitorPlugin,
                              FileLoggerPlugin>();

        // ApiPlugin Registration
        services.AddHttpClient<ApiPlugin>();

        services.AddSingleton<IMonitorPlugin>(
            provider =>
                provider.GetRequiredService<ApiPlugin>());

        // Monitor Manager
        services.AddSingleton<MonitorManager>();
    })
    .Build();

Console.WriteLine("=================================");
Console.WriteLine("      SYSTEM MONITOR STARTED     ");
Console.WriteLine("=================================");

var manager =
    host.Services.GetRequiredService<MonitorManager>();

await manager.StartAsync();