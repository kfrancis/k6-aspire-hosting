using K6.Hosting.Aspire;
using K6.Hosting.Outputs.Grafana;
using K6.Hosting.Outputs.InfluxDB;

var builder = DistributedApplication.CreateBuilder(args);

var influxDb = builder.AddInfluxDb("influxdb");

var api = builder.AddProject<Projects.GrafanaInfluxDbSample_ApiService>("api");

// Setup K6 with Grafana and InfluxDB
builder.AddK6("k6")
    .WithScript("./scripts/test.js")
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
