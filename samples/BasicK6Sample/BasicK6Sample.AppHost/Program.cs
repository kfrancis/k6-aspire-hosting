using K6.Hosting.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.BasicK6Sample_ApiService>("basick6sample-apiservice");

var k6 = builder.AddK6("k6", options =>
    {
        options.ScriptDirectory = "./scripts";
        options.ScriptFileName = "test.js";
    })
    .WithApiEndpoint(api);

builder.Build().Run();
