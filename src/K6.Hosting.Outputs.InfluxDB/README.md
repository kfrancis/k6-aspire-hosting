# CSS.K6.Hosting.Outputs.InfluxDB

InfluxDB output provider for storing and querying k6 load test metrics in .NET Aspire applications.

## Overview

This package extends the k6 integration for .NET Aspire by adding InfluxDB support. It enables you to store your load testing metrics in InfluxDB for efficient time-series data storage and analysis.

## Features

- Add InfluxDB container to your Aspire application
- Configure k6 to output metrics to InfluxDB
- Integration with Grafana for visualization
- Support for custom InfluxDB configurations

## Installation

```bash
dotnet add package CSS.K6.Hosting.Outputs.InfluxDB
```

## Requirements

- .NET 8.0 or .NET 9.0
- .NET Aspire
- CSS.K6.Hosting.Aspire
- Docker (for running the InfluxDB container)

## Usage

Add InfluxDB output to your k6 load testing setup:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Add your API project
var apiProject = builder.AddProject<Projects.MyApi>("api");

// Add k6 with InfluxDB output
builder.AddK6("load-testing", options => 
{
    options.ScriptDirectory = "scripts";
    options.ScriptFileName = "loadtest.js";
})
.WithApiEndpoint(apiProject)
.WithInfluxDbOutput();

// Add Grafana with InfluxDB datasource (optional)
// Requires CSS.K6.Hosting.Outputs.Grafana package
builder.WithGrafanaDashboard()
      .WithGrafanaInfluxDbDatasource();

// The application builder and services configuration continues...
```

### Options

- **ConnectionString**: Custom connection string for InfluxDB
- **ResourceName**: Name for the InfluxDB resource

## Related Packages

- **CSS.K6.Hosting.Core**: Core components for k6 integration
- **CSS.K6.Hosting.Aspire**: Main integration for k6 in Aspire
- **CSS.K6.Hosting.Outputs.Grafana**: Grafana support for visualizing metrics

## License

MIT