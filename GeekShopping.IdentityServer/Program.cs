using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace GeekShopping.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<MySQLContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("MySQLConnection"),
                    new MySqlServerVersion(new Version(8, 2, 0))
                )
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MySQLContext>().AddDefaultTokenProviders();

            var builderServices = builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.EmitStaticAudienceClaim = true;
            }).AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
              .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
              .AddInMemoryClients(IdentityConfiguration.Clients)
              .AddAspNetIdentity<ApplicationUser>();

            //builder.Services.AddScoped<IDBInitializer, DbInitializer>();
            //builder.Services.AddScoped<IProfileService, ProfileService>();

            builderServices.AddDeveloperSigningCredential();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            //var initializer = app.Services.CreateScope().ServiceProvider.GetService<IDBInitializer>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            //initializer.Initialize();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}