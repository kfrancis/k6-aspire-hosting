# CSS.K6.Hosting.Aspire

Integration for running Grafana k6 load tests within .NET Aspire applications.

## Overview

This package enables seamless integration of k6 load testing into your .NET Aspire applications. It provides extension methods for the Aspire `IDistributedApplicationBuilder` that make it easy to add and configure k6 resources.

## Features

- Add k6 load testing containers to your Aspire application
- Reference API projects for testing
- Configure k6 test scripts
- Extensible output system through additional packages

## Installation

```bash
dotnet add package CSS.K6.Hosting.Aspire
```

## Requirements

- .NET 8.0 or .NET 9.0
- .NET Aspire
- Docker (for running the k6 container)

## Usage

Basic usage to add k6 load testing to an Aspire application:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Add your API project
var apiProject = builder.AddProject<Projects.MyApi>("api");

// Add k6 for load testing
builder.AddK6("load-testing", options => 
{
    options.ScriptDirectory = "scripts";
    options.ScriptFileName = "loadtest.js";
})
.WithApiEndpoint(apiProject);

// The application builder and services configuration continues...
```

### Options

- **ScriptDirectory**: Directory containing k6 test scripts
- **ScriptFileName**: The main k6 script file to execute
- **ImageRegistry**: (Optional) Container registry for the k6 image
- **ImageName**: (Optional) Name of the k6 container image
- **ImageTag**: (Optional) Tag of the k6 container image

## Related Packages

- **CSS.K6.Hosting.Outputs.Grafana**: For visualizing k6 test results in Grafana
- **CSS.K6.Hosting.Outputs.InfluxDB**: For storing k6 test metrics in InfluxDB

## License

MIT