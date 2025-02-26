# CSS.K6.Hosting.Core

Core components for integrating Grafana k6 load testing into .NET Aspire applications.

## Overview

This package provides the fundamental abstractions and interfaces necessary for integrating k6 load testing tools within .NET Aspire applications. It serves as the foundation for the other k6 integration packages.

## Features

- Core abstractions for k6 resource management in .NET Aspire
- Support for configuring k6 test scripts
- Extensible output provider system
- Interface definitions for data sources and result outputs

## Installation

```bash
dotnet add package CSS.K6.Hosting.Core
```

## Requirements

- .NET 8.0 or .NET 9.0
- .NET Aspire

## Usage

This package is not typically used directly but serves as a foundation for other packages in the k6 integration suite. If you're looking to add k6 load testing to your Aspire application, consider using the `CSS.K6.Hosting.Aspire` package instead.

```csharp
// Core entities and interfaces will be referenced by your Aspire applications
// but usually through the extension methods provided by CSS.K6.Hosting.Aspire
```

## License

MIT