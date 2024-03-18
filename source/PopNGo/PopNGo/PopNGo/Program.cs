using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
using Microsoft.Extensions.Hosting;

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
        builder.Services.AddScoped<IEventHistoryRepository, EventHistoryRepository>();
        builder.Services.AddScoped<IPgUserRepository, PgUserRepository>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();
        builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();
        builder.Services.AddScoped<IEventRepository, EventRepository>();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<PopNGoUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
                    new TokenProviderDescriptor(
                    typeof(CustomEmailConfirmationTokenProvider<PopNGoUser>)));
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        
        builder.Services.AddTransient<CustomEmailConfirmationTokenProvider<PopNGoUser>>();
        builder.Services.AddTransient<IEmailSender, EmailSender>();
        builder.Services.AddHostedService<TimedEmailService>();

        builder.Services.AddControllersWithViews();

        // Add Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
        });

        var app = builder.Build();

        SeedData(app).Wait();


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

    public static async Task SeedData(WebApplication app) {
        using (var scope = app.Services.CreateScope()) {
            PopNGoDB _popNGoDBContext = scope.ServiceProvider.GetRequiredService<PopNGoDB>();
            UserManager<PopNGoUser> userSeeder = scope.ServiceProvider.GetRequiredService<UserManager<PopNGoUser>>();
            RoleManager<IdentityRole> roleSeeder = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var userExists = await userSeeder.FindByNameAsync("admin@popngo.com");
            if (userExists == null) {
                var user = Activator.CreateInstance<PopNGoUser>();
                user.UserName = "admin@popngo.com";
                user.Email = "popngo.wou@gmail.com";
                user.EmailConfirmed = true;
                user.FirstName = "The";
                user.LastName = "Admin";

                await userSeeder.CreateAsync(user, configuration["AdminPW"]);
                PgUser newUser = new()
                {
                    AspnetuserId = user.Id
                };
                _popNGoDBContext.PgUsers.Add(newUser);
                await _popNGoDBContext.SaveChangesAsync();
            }

            var roleExists = await roleSeeder.RoleExistsAsync("Admin");
            if (!roleExists) {
                await roleSeeder.CreateAsync(new IdentityRole("Admin"));
            }
            var roleUserExists = await roleSeeder.RoleExistsAsync("User");
            if (!roleUserExists) {
                await roleSeeder.CreateAsync(new IdentityRole("User"));
            }

            var adminUser = await userSeeder.FindByNameAsync("admin@popngo.com");
            if (!await userSeeder.IsInRoleAsync(adminUser, "Admin")) {
                await userSeeder.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
