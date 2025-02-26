# k6-aspire-hosting

A suite of .NET libraries for integrating Grafana k6 load testing into .NET Aspire applications.

[![NuGet](https://img.shields.io/nuget/v/CSS.K6.Hosting.Aspire.svg)](https://www.nuget.org/packages/CSS.K6.Hosting.Aspire)
[![Build Status](https://github.com/kfrancis/k6-aspire-hosting/workflows/publish/badge.svg)](https://github.com/kfrancis/k6-aspire-hosting/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Overview

This repository contains a collection of libraries designed to seamlessly integrate Grafana k6 load testing into .NET Aspire applications. It enables developers to define, run, and visualize load tests as part of their Aspire application infrastructure.

## Packages

| Package | Description | NuGet |
| ------- | ----------- | ----- |
| [CSS.K6.Hosting.Core](./src/K6.Hosting.Core) | Core components for k6 integration | [![NuGet](https://img.shields.io/nuget/v/CSS.K6.Hosting.Core.svg)](https://www.nuget.org/packages/CSS.K6.Hosting.Core) |
| [CSS.K6.Hosting.Aspire](./src/K6.Hosting.Aspire) | Main integration for k6 in Aspire | [![NuGet](https://img.shields.io/nuget/v/CSS.K6.Hosting.Aspire.svg)](https://www.nuget.org/packages/CSS.K6.Hosting.Aspire) |
| [CSS.K6.Hosting.Outputs.Grafana](./src/K6.Hosting.Outputs.Grafana) | Grafana dashboards for visualizing metrics | [![NuGet](https://img.shields.io/nuget/v/CSS.K6.Hosting.Outputs.Grafana.svg)](https://www.nuget.org/packages/CSS.K6.Hosting.Outputs.Grafana) |
| [CSS.K6.Hosting.Outputs.InfluxDB](./src/K6.Hosting.Outputs.InfluxDB) | InfluxDB support for storing metrics | [![NuGet](https://img.shields.io/nuget/v/CSS.K6.Hosting.Outputs.InfluxDB.svg)](https://www.nuget.org/packages/CSS.K6.Hosting.Outputs.InfluxDB) |

## Quick Start

### 1. Install the package

```bash
dotnet add package CSS.K6.Hosting.Aspire
```

For visualization with Grafana:

```bash
dotnet add package CSS.K6.Hosting.Outputs.Grafana
```

For metrics storage with InfluxDB:

```bash
dotnet add package CSS.K6.Hosting.Outputs.InfluxDB
```

### 2. Add k6 to your Aspire application

Create a k6 test script file in your project (e.g., `scripts/load-test.js`):

```javascript
import http from 'k6/http';
import { sleep } from 'k6';

export default function() {
  http.get('http://api/weatherforecast');
  sleep(1);
}
```

Add k6 to your Aspire application:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Add your API project
var apiProject = builder.AddProject<Projects.MyApi>("api");

// Add k6 for load testing
builder.AddK6("load-testing", options => 
{
    options.ScriptDirectory = "scripts";
    options.ScriptFileName = "load-test.js";
})
.WithApiEndpoint(apiProject)
.WithInfluxDbOutput()
.WithGrafanaDashboard();

var app = builder.Build();
await app.RunAsync();
```

## Features

- **Simple Integration**: Add k6 load testing to .NET Aspire with minimal configuration
- **API Testing**: Automatically test APIs defined in your Aspire application
- **Visualization**: Built-in support for Grafana dashboards
- **Metric Storage**: Store test results in InfluxDB for analysis
- **Docker-based**: Leverages Docker containers for consistent testing environments

## Requirements

- .NET 8.0 or .NET 9.0
- .NET Aspire
- Docker (for running containers)

## Documentation

For more detailed documentation, see the README files in each package directory:

- [K6.Hosting.Core](./src/K6.Hosting.Core/README.md)
- [K6.Hosting.Aspire](./src/K6.Hosting.Aspire/README.md)
- [K6.Hosting.Outputs.Grafana](./src/K6.Hosting.Outputs.Grafana/README.md)
- [K6.Hosting.Outputs.InfluxDB](./src/K6.Hosting.Outputs.InfluxDB/README.md)

## Examples

See the [samples](./samples) directory for sample applications demonstrating how to use these libraries.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
