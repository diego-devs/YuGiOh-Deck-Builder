using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YGODeckBuilder.Data;
using YGODeckBuilder.DataProviders;
using YGODeckBuilder.Interfaces;

namespace YGODeckBuilder
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; // appsettings.json
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the configuration
            services.Configure<AppSettingsReader>(Configuration);
            services.AddControllers();
            // Register IDeckUtility and its implementation
            services.AddScoped<IDeckUtility, DeckUtility>();
            services.AddHttpContextAccessor();
            services.AddSession();

            // Access configuration values
            var decksFolderPath = Configuration["Paths:DecksFolderPath"];
            var cardIdsFilePath = Configuration["Paths:CardIdsFilePath"];
            var connectionString = Configuration["ConnectionStrings:YGODatabase"];

            // Use the configuration values as needed
            Console.WriteLine($"Decks Folder Path: {decksFolderPath}");
            Console.WriteLine($"Card IDs File Path: {cardIdsFilePath}");
            Console.WriteLine($"Connetion String Path: {connectionString}");

            services.AddSingleton<ICardsProvider, YgoAPIProvider>();

            services.AddDbContext<YgoContext>(options => options.UseSqlServer(connectionString));

            services.AddRazorPages();
           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers(); // Map API endpoints
            });
        }
    }
}
