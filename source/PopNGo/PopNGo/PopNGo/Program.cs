using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopNGo.Areas.Identity.Data;
using PopNGo.Data;
using PopNGo.Models;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Services;
using Microsoft.OpenApi.Models;
using PopNGo.DAL.Abstract;
using PopNGo.DAL.Concrete;

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
        // var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection") ?? throw new InvalidOperationException("Connection string 'IdentityConnection' not found.");
        // var identityConnection = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("IdentityConnectionAzure"))
        // {
        //     Password = builder.Configuration["PopNGo:DBPassword"]
        // };
        // var identityConnectionString = identityConnection.ConnectionString;
        var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");
        // var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnectionAzure");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options
            .UseSqlServer(identityConnectionString)
            .UseLazyLoadingProxies());
        
        // var serverConnectionString = builder.Configuration.GetConnectionString("ServerConnection") ?? throw new InvalidOperationException("Connection string 'ServerConnection' not found.");
        // var serverConnection = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("ServerConnectionAzure"))
        // {
        //     Password = builder.Configuration["PopNGo:DBPassword"]
        // };
        // var serverConnectionString = serverConnection.ConnectionString;
        var serverConnectionString = builder.Configuration.GetConnectionString("ServerConnection");
        // var serverConnectionString = builder.Configuration.GetConnectionString("ServerConnectionAzure");
        builder.Services.AddDbContext<PopNGoDB>(options => options
            .UseSqlServer(serverConnectionString)
            .UseLazyLoadingProxies());
        builder.Services.AddScoped<DbContext,PopNGoDB>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<PopNGoUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
        })
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
            // app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
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
