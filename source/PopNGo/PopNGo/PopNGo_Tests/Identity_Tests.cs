using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using PopNGo.Data;
using PopNGo.Models;
using System.Diagnostics;
using System.Net;
using PopNGo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace PopNGo_Tests;

/**
 * This is the recommended way to test using the in-memory db.  Seed the db and then write your tests based only on the seed
 * data + anything else you do to it.  No other tests will modify the db for that test.  Every test gets a brand new seeded db.
 * 
 */
public class Identity_Tests
{
    private static readonly string _seedFile = @"..\..\..\..\PopNGo\Data\Scripts\IDENTITYSEED.sql";  // relative path from where the executable is: bin/Debug/net7.0
    private static readonly string _upFile = @"..\..\..\..\PopNGo\Data\Scripts\IDENTITYUP.sql";  // relative path from where the executable is: bin/Debug/net7.0

    // Create this helper like this, for whatever context you desire
    private static readonly InMemoryDbHelper<ApplicationDbContext> _dbHelper = new(_seedFile, _upFile, DbPersistence.OneDbPerTest);
    private readonly UserManager<PopNGoUser> _userManager = new(new UserStore<PopNGoUser>(_dbHelper.GetContext()), null, null, null, null, null, null, null, null);

    [Test]
    public async Task ApplicationContext_ChangeNotificationEmail_IsSuccessful()
    {
        // And then get your context
        using ApplicationDbContext context = _dbHelper.GetContext();

        // Arrange
        PopNGoUser expected = await _userManager.FindByNameAsync("Tristan Goucher");

        // Act
        expected.NotificationEmail = "JohnDoe@gmail.com";
        await _userManager.UpdateAsync(expected);

        // Assert
        Assert.That(context.Users.Where(u => u.UserName == "Tristan Goucher").First().NotificationEmail
        , Is.EqualTo(expected.NotificationEmail));
    }

    [Test]
    public void ApplicationContext_HasBeenSeeded()
    {
        ApplicationDbContext context = _dbHelper.GetContext();

        Console.WriteLine("Users: " + context.Users.Count());
        Assert.That(context.Users.Count(), Is.EqualTo(4));
    }

   
}