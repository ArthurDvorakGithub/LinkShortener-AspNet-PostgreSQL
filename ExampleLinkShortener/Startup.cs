using ExampleLinkShortener.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            // Facebook login
            services.AddAuthentication()
             .AddFacebook(facebookOptions =>
             {
                 facebookOptions.AppId = "226133136140998";
                 facebookOptions.AppSecret = "98a9a17feb641b6ec11e1dcfcdd65729";
                 facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";

             });

            // Google login
            services.AddAuthentication()
            .AddGoogle(options =>
             {
                 IConfigurationSection googleAuthNSection =
                       Configuration.GetSection("Authentication:Google");
                 options.ClientId = "102100967624-evq33higtn1i0036nfvdacjmp57a5sve.apps.googleusercontent.com";
                 options.ClientSecret = "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";
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


        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    // подключение аутентификации
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
