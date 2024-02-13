using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PopNGo.Data;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Services;
using Microsoft.OpenApi.Models;

namespace PopNGo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //REST API setup for Real Time Event Search API
        string realTimeEventSearchApiKey = builder.Configuration["RealTimeEventSearchApiKey"];
        string realTimeEventSearchUrl = "https://real-time-events-search.p.rapidapi.com/";

        builder.Services.AddHttpClient<IRealTimeEventSearchService, RealTimeEventSearchService>((httpClient, services) =>
        {
            httpClient.BaseAddress = new Uri(realTimeEventSearchUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", realTimeEventSearchApiKey); // Set API key
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "real-time-events-search.p.rapidapi.com");
            return new RealTimeEventSearchService(httpClient, services.GetRequiredService<ILogger<RealTimeEventSearchService>>());
        });


        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();


        builder.Services.AddControllersWithViews();

        // Add Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
