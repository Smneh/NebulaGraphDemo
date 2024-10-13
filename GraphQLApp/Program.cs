using GraphQL.Query;
using NebulaGraphDemo.Repository.Data;
using NebulaGraphDemo.Repository.Sessions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddSingleton<NebulaSessionManager>(sp => 
    new NebulaSessionManager("192.168.111.141", 9669));


// Register GraphQL services
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>(); // Ensure your Query class is correctly referenced

var app = builder.Build();

// Map the GraphQL endpoint
app.MapGraphQL(); 

// Serve a simple message at the root path
app.MapGet("/", () => "Use /graphql to access the GraphQL endpoint.");


// Ensure the necessary space exists
using (var scope = app.Services.CreateScope())
{
    var sessionManager = scope.ServiceProvider.GetRequiredService<NebulaSessionManager>(); // Assume you have a service for managing Nebula Graph
    await sessionManager.EnsureSpaceExistsAsync("SamanehSpace");
    await sessionManager.UseSpaceAsync("SamanehSpace");
}

app.Run();