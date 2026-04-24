using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YGODeckBuilder.Data;
using YGODeckBuilder.Data.EntityModels;
using YGODeckBuilder.DataProviders;
using YGODeckBuilder.Interfaces;

namespace YGODeckBuilder
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
            services.Configure<AppSettingsReader>(Configuration);
            services.AddControllers()
                .AddJsonOptions(opt =>
                    opt.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

            services.AddScoped<IDeckUtility, DeckUtility>();
            services.AddHttpContextAccessor();
            services.AddSession();

            var connectionString = Configuration["ConnectionStrings:YGODatabase"];

            services.AddHttpClient("ygoprodeck");
            services.AddSingleton<ICardsProvider, YgoAPIProvider>();

            services.AddDbContext<YgoContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly("YugiohDB")));

            services.AddScoped<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = "/Account/Login";
                    opt.LogoutPath = "/Account/Logout";
                    opt.ExpireTimeSpan = TimeSpan.FromDays(7);
                    opt.SlidingExpiration = true;
                });

            services.AddAuthorization();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
