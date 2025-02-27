using K6.Hosting.Aspire;
using K6.Hosting.Outputs.Grafana;
using K6.Hosting.Outputs.InfluxDB;

var builder = DistributedApplication.CreateBuilder(args);

var influxDb = builder.AddInfluxDb("influxdb");

var api = builder.AddProject<Projects.GrafanaInfluxDbSample_ApiService>("basick6sample-apiservice");

var k6 = builder.AddK6("k6", options =>
    {
        options.ScriptDirectory = "./scripts";
        options.ScriptFileName = "test.js";
    })
    .WithReference(influxDb).WaitFor(influxDb)
    .WithApiEndpoint(api)
    .WithInfluxDbOutput(influxDb)
    .WithGrafanaDashboard(options =>
    {
        options.DashboardsPath = "./dashboards";
        options.DashboardConfigPath = "./grafana-dashboard.yaml";
    })
    .WithGrafanaInfluxDbDatasource(influxDb, "./grafana-datasource.yaml");

builder.Build().Run();
