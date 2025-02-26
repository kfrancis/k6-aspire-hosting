# CSS.K6.Hosting.Outputs.Grafana

Grafana dashboard integration for visualizing k6 load test results in .NET Aspire applications.

## Overview

This package extends the k6 integration for .NET Aspire by adding Grafana dashboard support. It enables you to visualize and analyze your load testing results with pre-configured Grafana dashboards.

## Features

- Add Grafana container to your Aspire application
- Automatic configuration of Grafana datasources
- Built-in k6 performance dashboards
- Support for custom dashboards
- Integration with various data sources (requires additional providers)

## Installation

```bash
dotnet add package CSS.K6.Hosting.Outputs.Grafana
```

## Requirements

- .NET 8.0 or .NET 9.0
- .NET Aspire
- CSS.K6.Hosting.Aspire
- Docker (for running the Grafana container)

## Usage

Add Grafana visualization to your k6 load testing setup:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Add your API project
var apiProject = builder.AddProject<Projects.MyApi>("api");

// Add k6 with Grafana visualization
builder.AddK6("load-testing", options => 
{
    options.ScriptDirectory = "scripts";
    options.ScriptFileName = "loadtest.js";
})
.WithApiEndpoint(apiProject)
.WithGrafanaDashboard(options => 
{
    // Customize Grafana settings (optional)
    options.ResourceName = "grafana";
    options.DashboardsPath = "dashboards";
});

// The application builder and services configuration continues...
```

### Options

- **ResourceName**: Name for the Grafana resource
- **DashboardsPath**: Custom path to Grafana dashboards
- **DatasourceConfigPath**: Path to custom datasource config
- **DashboardConfigPath**: Path to custom dashboard config
- **ImageConfig**: Container image configuration

## Related Packages

- **CSS.K6.Hosting.Core**: Core components for k6 integration
- **CSS.K6.Hosting.Aspire**: Main integration for k6 in Aspire
- **CSS.K6.Hosting.Outputs.InfluxDB**: InfluxDB support for storing metrics

## License

MIT