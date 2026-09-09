var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.MiniEnv_Api>("api");
var workers = builder.AddProject<Projects.MiniEnv_Workers>("workers");

builder.Build().Run();
