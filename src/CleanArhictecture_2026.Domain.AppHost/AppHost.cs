var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CleanArhictecture_2026_WebAPI>("cleanarhictecture-2026-webapi");

builder.Build().Run();
