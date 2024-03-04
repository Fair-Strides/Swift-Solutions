using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.Models;

namespace PopNGo.Services;

public class TimedEmailService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<TimedEmailService> _logger;
    private Timer _timer = null;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IEmailSender _emailSender;
    private readonly string _logoBase64 = null;

    public TimedEmailService(IServiceScopeFactory serviceScopeFactory, ILogger<TimedEmailService> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _emailSender = emailSender;

        // Convert the logo to base64
        string logoPath = "wwwroot/media/images/logo.svg";
        byte[] imageArray = File.ReadAllBytes(logoPath);
        _logoBase64 = Convert.ToBase64String(imageArray);
        
        _logger.LogInformation("Timed Hosted Service running.");
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        DateTime current = DateTime.Now;
        DateTime next = new DateTime(current.Year, current.Month, current.Day, 0, 0, 0).AddDays(1);
        // DateTime next = new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second).AddSeconds(10);
        // _timer = new Timer(DoWork, null, TimeSpan.FromSeconds((next - current).TotalSeconds), TimeSpan.FromSeconds(5));
        _timer = new Timer(DoWork, null, TimeSpan.FromSeconds((next - current).TotalSeconds), TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        var count = Interlocked.Increment(ref executionCount);
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            IPgUserRepository _userRepo = scope.ServiceProvider.GetRequiredService<IPgUserRepository>();
            IFavoritesRepository _favoritesRepo = scope.ServiceProvider.GetRequiredService<IFavoritesRepository>();
            UserManager<PopNGoUser> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<PopNGoUser>>();

            List<PgUser> userList = _userRepo.GetAll().ToList();
            string users = "";
            foreach (var user in userList)
            {
                // Console.WriteLine($"Processing emails for {user.Id} - {user.AspnetuserId}");
                PopNGoUser popNGoUser = await _userManager.FindByIdAsync(user.AspnetuserId);
                
                if(popNGoUser != null) {
                    users += $"{user.Id} - {popNGoUser.UserName},\n";
                    // Console.WriteLine($"User: {popNGoUser.UserName}");
                    // if(popNGoUser != null && popNGoUser.FirstName == "Tristan")
                    // {
                    
                    List<Models.DTO.Event> favorites = _favoritesRepo.GetUserFavorites(user.Id);

                    List<Models.DTO.Event> weekBeforeFaves = new List<Models.DTO.Event>();
                    List<Models.DTO.Event> dayBeforeFaves = new List<Models.DTO.Event>();
                    List<Models.DTO.Event> dayOfFaves = new List<Models.DTO.Event>();

                    DateTime now = DateTime.Now.Date;
                    foreach (var favorite in favorites)
                    {
                        DateTime eventStart = favorite.EventDate.Date;
                        if (eventStart == now.AddDays(7) && popNGoUser.NotifyWeekBefore)
                        {
                            weekBeforeFaves.Add(favorite);
                        }
                        else if (eventStart == now.AddDays(1) && popNGoUser.NotifyDayBefore)
                        {
                            dayBeforeFaves.Add(favorite);
                        }
                        else if (eventStart == now && popNGoUser.NotifyDayOf)
                        {
                            dayOfFaves.Add(favorite);
                        }
                    }

                    string emailBody = $"<div style=\"margin: 0px; padding: 0px;\">";
                    // emailBody += $"<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" bgcolor=\"#d35400\"><tr><td width=\"100%\" align=\"center\"><img src=\"https://popngostorage.blob.core.windows.net/images/logo.svg\" alt=\"PopNGo Logo\" width=\"100\" height=\"100\" style=\"display: block;\" /></td></tr>";
                    // emailBody += $"<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" bgcolor=\"#d35400\"><tr><td width=\"100%\" align=\"center\"><img src=\"data:image/svg+xml;base64,{_logoBase64}\" alt=\"PopNGo Logo\" width=\"100\" height=\"100\" style=\"display: block;\" /></td></tr>";
                    emailBody += "<tr><td width=\"100%\" align=\"center\"><h1 style=\"color: #ffffff; font-family: Arial, sans-serif; margin-bottom:0px;\">Pop-n-Go</h1></td></tr>";
                    emailBody += $"<tr><td width=\"90%\" style=\"padding: 15px; padding-left: 5%; padding-right: 5%;\"><p style=\"color: #ffffff; font-family: Arial, sans-serif;\">Hello, {popNGoUser.FirstName},</p><p style=\"color: #ffffff; font-family: Arial, sans-serif;\">Thank you for using Pop-n-Go's services. We've collected a list of events you showed interest in and wanted to let you know what's ahead in case you want to alter your plans for them.</p></td></tr>";
                    emailBody += "<tr><td width=\"100%\" align=\"center\"><h2 style=\"color: #ffffff; font-family: Arial, sans-serif;\">Your Event Reminders</h2></td></tr>";

                    // Today's events
                    emailBody += "<hr style=\"border-top: 1px solid #d6d6d6; line-height: 1px;\" width=\"90%\" align=\"center\" />";
                    emailBody += "<tr><td width=\"100%\" align=\"center\"><h2 style=\"color: #ffffff; font-family: Arial, sans-serif;\">Today:</h3></td></tr>";
                    if (dayOfFaves.Count > 0)
                    {
                        emailBody += BuildEvents(dayOfFaves);
                    } else {
                        emailBody += "<tr><td width=\"100%\" align=\"center\"><p style=\"color: #ffffff; font-family: Arial, sans-serif;\">No events to remind you about</p></td></tr>";
                    }

                    // Tomorrow's events
                    emailBody += "<hr style=\"border-top: 1px solid #d6d6d6; line-height: 1px;\" width=\"90%\" align=\"center\" />";
                    emailBody += "<tr><td width=\"100%\" align=\"center\"><h2 style=\"color: #ffffff; font-family: Arial, sans-serif;\">Tomorrow:</h3></td></tr>";
                    if (dayBeforeFaves.Count > 0)
                    {
                        emailBody += BuildEvents(dayBeforeFaves);
                    } else {
                        emailBody += "<tr><td width=\"100%\" align=\"center\"><p style=\"color: #ffffff; font-family: Arial, sans-serif;\">No events to remind you about</p></td></tr>";
                    }

                    // Events from a week from now
                    emailBody += "<hr style=\"border-top: 1px solid #d6d6d6; line-height: 1px;\" width=\"90%\" align=\"center\" />";
                    emailBody += "<tr><td width=\"100%\" align=\"center\"><h2 style=\"color: #ffffff; font-family: Arial, sans-serif;\">A week from now:</h3></td></tr>";
                    if (weekBeforeFaves.Count > 0)
                    {
                        emailBody += BuildEvents(weekBeforeFaves);
                    } else {
                        emailBody += "<tr><td width=\"100%\" align=\"center\"><p style=\"color: #ffffff; font-family: Arial, sans-serif;\">No events to remind you about</p></td></tr>";
                    }

                    emailBody += "<hr style=\"border-top: 1px solid #d6d6d6; line-height: 1px;\" width=\"90%\" align=\"center\" />";

                    emailBody += "</table></div>";

                    users += $"Processing emails for {user.AspnetuserId} - {popNGoUser.UserName},\n";
                    await _emailSender.SendEmailAsync(popNGoUser.NotificationEmail, "Your Event Reminders", emailBody);
                    // }
                }
            }

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}, Time: {time}\nUsers:\n{users}", count, DateTime.Now, users);
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public string BuildEvents(List<Models.DTO.Event> events)    
    {
        string emailBody = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"1\" width=\"90%\" align=\"center\" bgcolor=\"#ff8f45\" style=\"border-color: rgba(163, 65, 0, 0.5); border-radius: 5px; margin-bottom:10px;\">";
        foreach (var e in events)
        {
            if(e.EventDate.Hour == 0)
            {
                e.EventDate = e.EventDate.AddHours(12);
            }
            string time = e.EventDate.TimeOfDay.Hours > 12 ? $"{e.EventDate.TimeOfDay.Hours - 12}:{e.EventDate.TimeOfDay.Minutes.ToString("D2")} PM" : $"{e.EventDate.TimeOfDay.Hours}:{e.EventDate.TimeOfDay.Minutes.ToString("D2")} AM";
            string locationURL = $"https://www.google.com/maps/place/{e.EventLocation.Replace(" ", "+")}";
            emailBody += $"<tr><td width=\"100%\" style=\"border-width: 0px; padding-left: 10px;\"><h3 style=\"color: #a73f00; font-family: Arial, sans-serif;\">{e.EventName} - {time}</h4></td></tr>";
            emailBody += $"<tr><td width=\"100%\" style=\"border-width: 0px; padding-left: 10px; padding-bottom: 5px;\"><a href=\"{locationURL}\">{e.EventLocation}</a></td></tr>";
            emailBody += $"<tr><td width=\"100%\" style=\"border-width: 0px; padding-left: 10px; padding-bottom: 10px;\"><p style=\"color: #a73f00; font-family: Arial, sans-serif; margin: 0px\">{e.EventDescription}</p></td></tr>";

            if(events.IndexOf(e) < events.Count - 1)
            {
                emailBody += "<tr><td style=\"border-width:0px; border-bottom: 1px solid #d6d6d6; line-height: 1px;\" width=\"100%\" align=\"center\"></td></tr>";
            }
        }
        emailBody += "</table>";
        return emailBody;
    }
}