using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddContainer("smtp4dev", "rnwood/smtp4dev")
    .WithHttpEndpoint(port: 2525, targetPort: 80)
    .WithHttpEndpoint(port: 25, targetPort: 25);
    //.WithEndpoint(2525, 80)
    //.WithEndpoint(25, 25);

builder
    .AddProject<Projects.OverdueBookReporter>("main-reporter-job");

builder.Build().Run();
