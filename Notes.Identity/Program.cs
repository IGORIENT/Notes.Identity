using Notes.Identity;
using Notes.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Notes.Identity.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//������� ������ ����������� �� appsettings
var connectionString = builder.Configuration.GetValue<string>("DbConnection");

// builder.Services - ��������� ���� ��������, ������� ���� � DI
// ��������� ��-��������
builder.Services.AddDbContext<AuthDbContext>(options =>  //��������� � ��������� ������ ���� AuthDbContext;
{
    options.UseSqlite(connectionString);
});

// ��������� ASP Identity � DI, � ����������� ��� ��� ������ � ���� ����� ������������
builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 4;  // ����������� ����� ������
    config.Password.RequireDigit = false;  // ����� ������ ������ ��� ����.
    config.Password.RequireNonAlphanumeric = false; //����� ������ ������ ��� ������ �������� _*"@ � �.�.
    config.Password.RequireUppercase = false; // ����� ������ ������ ��� ���������
})
    // ������ ���� �������������, ����, ������ � �.�. � ���� ������,
    // ��������� ��� AuthDbContext � Entity Framework Core�.
    // �������� ����� ������ �������� � ��������������� �������
    .AddEntityFrameworkStores<AuthDbContext>()

    // ����������� ����������� ���������� ����������� ����� ��� ������������� � ������� �������.
    .AddDefaultTokenProviders();

//��������� ��� ��, ��� ������� � Cpnfiguration.cs
builder.Services.AddIdentityServer()
    .AddAspNetIdentity<AppUser>()  //��������� ��� ������� ASP.NET Identity � ����� ������������ AppUser ��� �����, �������� �������, ����� � ��������.
    .AddInMemoryApiResources(Configuration.ApiResiurces) 
    .AddInMemoryIdentityResources(Configuration.IdentityResources)
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddInMemoryClients(Configuration.Clients)
    .AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Notes.Identity.Cookie";
    config.LoginPath = "/Auth/Login";
    config.LogoutPath = "/Auth/Logout";
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    {
        try
        {
            var context = serviceProvider.GetRequiredService<AuthDbContext>(); //�����, ������� ������ �� Scope ������ ������� ����.
                                                                               //���� ������ ������� ��� � �������� ����������.
            DbInitializer.Initialize(context);
        }
        catch(Exception exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>(); //���� ����� �������� �� ����� ������ program
                                                                                 //��� ���� ������������ ���� � ��� �� serviceProvider, �� ������������� ������ ����.

            logger.LogError(exception, "An error occured while app initilization");
        }
    }
}

app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();

