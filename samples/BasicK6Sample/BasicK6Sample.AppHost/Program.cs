using K6.Hosting.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.BasicK6Sample_ApiService>("basick6sample-apiservice")
    .WithExternalHttpEndpoints();

var k6 = builder.AddK6("k6", options =>
    {
        options.ScriptDirectory = Path.GetFullPath("./scripts");
        options.ScriptFileName = "test.js";
    })
    .WaitFor(api)
    .WithApiEndpoint(api);

builder.Build().Run();
