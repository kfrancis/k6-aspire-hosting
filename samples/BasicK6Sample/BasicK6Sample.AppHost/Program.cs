using Aspire.Hosting;
using K6.Hosting.Aspire;
using K6.Hosting.Outputs.Aspire;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<BasicK6Sample_ApiService>("api")
    .WithExternalHttpEndpoints();

var collector = builder.AddOpenTelemetryCollector("otelcollector", "./otelconfig.yaml")
    .WithAppForwarding();

builder.AddK6("k6")
    .WithScript("./scripts/test.js")
    .WithApiEndpoint(api).WaitFor(api)
    .WithAspireMetrics(options =>
    {
        options.OtlpEndpoint = "http://otel-collector:4317";

    })
    .WaitFor(collector);

await builder.Build().RunAsync();
