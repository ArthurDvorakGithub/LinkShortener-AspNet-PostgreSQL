using ExampleLinkShortener.DataAccess;
using ExampleLinkShortener.DataAccess.Entities;
using ExampleLinkShortener.Models;
using ExampleLinkShortener.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExampleLinkShortener
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            // Facebook login
            services.AddAuthentication()
             .AddFacebook(facebookOptions =>
             {
                 facebookOptions.AppId = "x";
                 facebookOptions.AppSecret = "x";
                 facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";

             });

            // Google login
            services.AddAuthentication()
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                      Configuration.GetSection("Authentication:Google");
                options.ClientId = "x";
                options.ClientSecret = "x  ";
                //this function is get user google profile image
                options.Scope.Add("profile");
                options.Events.OnCreatingTicket = (context) =>
                {
                    var picture = context.User.GetProperty("picture").GetString();
                    context.Identity.AddClaim(new Claim("picture", picture));
                    return Task.CompletedTask;
                };

            });


            services.AddControllersWithViews();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shortener API", Version = "v1" });
            });

            services.AddControllersWithViews();

            services.AddScoped<IShortenerService, ShortenerService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<DbSeeder>();

        }

        public void Configure(IApplicationBuilder app, DbSeeder seeder, ApplicationContext context)
        {
            context.Database.EnsureCreated();
            //context.Database.Migrate();
            seeder.Seed().Wait();

            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shortener API v1"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
