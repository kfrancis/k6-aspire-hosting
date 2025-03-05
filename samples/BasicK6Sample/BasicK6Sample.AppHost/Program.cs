using K6.Hosting.Aspire;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<BasicK6Sample_ApiService>("api")
    .WithExternalHttpEndpoints();

builder.AddK6("k6")
    .WithScript("./scripts/test.js")
    .WithApiEndpoint(api)
    .WaitFor(api);

await builder.Build().RunAsync();
