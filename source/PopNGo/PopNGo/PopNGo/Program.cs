using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopNGo.Areas.Identity.Data;
using PopNGo.Data;
using PopNGo.Models;

namespace PopNGo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
        
        
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<PopNGoUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
        })
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
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
