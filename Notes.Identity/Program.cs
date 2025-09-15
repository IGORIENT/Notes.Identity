using Notes.Identity;
using Notes.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Notes.Identity.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//получаю строку подключения из appsettings
var connectionString = builder.Configuration.GetValue<string>("DbConnection");

// builder.Services - коллекция всех сервисов, которые есть в DI
// добавляем ДБ-контекст
builder.Services.AddDbContext<AuthDbContext>(options =>  //добавляет в контейнер сервис типа AuthDbContext;
{
    options.UseSqlite(connectionString);
});

// добавляет ASP Identity в DI, и настраивает его для работы с моим типом пользователя
builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 4;  // минимальная длина пароля
    config.Password.RequireDigit = false;  // можно писать пароль без цифр.
    config.Password.RequireNonAlphanumeric = false; //можно писать пароль без особых символов _*"@ и т.д.
    config.Password.RequireUppercase = false; // можно писать пароль без заглавных
})
    // «Храни всех пользователей, роли, пароли и т.п. в базе данных,
    // используя мой AuthDbContext и Entity Framework Core».
    // благодая этому данные попадают в переименованные таблицы
    .AddEntityFrameworkStores<AuthDbContext>()

    // «подключить стандартные генераторы одноразовых кодов для подтверждений и сбросов пароля».
    .AddDefaultTokenProviders();

//добавляем все то, что описали в Cpnfiguration.cs
builder.Services.AddIdentityServer()
    .AddAspNetIdentity<AppUser>()  //Используй мою систему ASP.NET Identity с типом пользователя AppUser для входа, проверки паролей, ролей и профилей».
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
            var context = serviceProvider.GetRequiredService<AuthDbContext>(); //метод, который достаёт из Scope объект нужного типа.
                                                                               //Если такого сервиса нет – выбросит исключение.
            DbInitializer.Initialize(context);
        }
        catch(Exception exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>(); //логи будут писаться от имени класса program
                                                                                 //оба раза используется один и тот же serviceProvider, но запрашиваются разные типы.

            logger.LogError(exception, "An error occured while app initilization");
        }
    }
}

app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();

