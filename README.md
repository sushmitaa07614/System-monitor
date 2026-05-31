# System Monitor

# Overview

System Monitor is a .NET 8 console application that tracks system resources such as CPU, RAM, and Disk usage in real time. The application displays the collected information in the console and supports a plugin-based architecture for extending functionality.

Currently implemented plugins:

* File Logger Plugin
* REST API Plugin

# Approach

The application was designed using a clean and modular architecture. System monitoring functionality is separated from plugin functionality using interfaces.

The ISystemMonitor interface is responsible for collecting system metrics, while the IMonitorPlugin interface allows new integrations to be added without changing the core monitoring logic.

Dependency Injection is used to manage services and plugins, making the application easier to maintain and extend.

# CPU Monitoring Challenges

The main challenge while implementing CPU monitoring was obtaining accurate CPU usage values. The Windows PerformanceCounter API does not return a valid reading immediately after initialization. The first value is usually inaccurate or zero.

To solve this, the counter is initialized first and a short delay is introduced before reading the actual CPU usage value.

Another challenge was that CPU monitoring APIs are platform-specific. To keep the application extensible, all Windows-specific monitoring code was isolated inside the `WindowsSystemMonitor` class, allowing future Linux or macOS implementations to be added without affecting the rest of the application.

# Technologies Used

* C# (.NET 8)
* Dependency Injection
* PerformanceCounter
* System.Management
* HttpClient
* JSON Configuration

# How to Run

Restore packages:

dotnet restore

Build the project:

dotnet build

Run the application:

dotnet run

## Future Improvements

* Linux and macOS monitoring support
* Plugin auto-discovery
* Retry mechanism for API calls
* Unit tests
* Structured logging

## Conclusion

This project demonstrates a simple but extensible system monitoring solution using modern .NET practices such as Dependency Injection, interface-based design, and plugin architecture.
