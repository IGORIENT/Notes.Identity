using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notes.Identity.Models;
using System.Data;

namespace Notes.Identity.Data
{
    public class AuthDbContext : IdentityDbContext<AppUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : //EF Core при запуске передаст сюда настройки (строку подключения, провайдер).
            base(options) { }


        protected override void OnModelCreating(ModelBuilder builder) //Это метод, где описываем правила для таблиц.
        {
            base.OnModelCreating(builder);  //base.OnModelCreating создаёт все таблицы, которые нужны для Identity (Users, Roles, Claims и т.д.).


            //По умолчанию EF Core/Identity создаёт таблицы с именами вроде AspNetUsers, AspNetRoles, …
            //Здесь вы переименовываете их в более короткие и понятные:
            //AspNetUsers → Users
            //AspNetRoles → Roles
            //и так далее.
            builder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));
            builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable(name: "UserClaim"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable(name: "UserTokens"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable(name: "UserClaims"));

            builder.ApplyConfiguration(new AppUserConfiguration());
        }
    }
}
