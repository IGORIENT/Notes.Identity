using Notes.Identity;

using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

//добавляем все то, что описали в Cpnfiguration.cs
builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(Configuration.ApiResiurces) 
    .AddInMemoryIdentityResources(Configuration.IdentityResources)
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddInMemoryClients(Configuration.Clients)
    .AddDeveloperSigningCredential();

var app = builder.Build();


app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();

