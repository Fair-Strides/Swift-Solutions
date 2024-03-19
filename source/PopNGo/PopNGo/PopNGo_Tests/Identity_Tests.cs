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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PopNGo_Tests;

/**
 * This is the recommended way to test using the in-memory db.  Seed the db and then write your tests based only on the seed
 * data + anything else you do to it.  No other tests will modify the db for that test.  Every test gets a brand new seeded db.
 * 
 */
public class Identity_Tests
{
    private static readonly string _seedFile = @"..\..\..\..\PopNGo\Data\Scripts\Testing\IDENTITYSEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    // Create this helper like this, for whatever context you desire
    private static readonly InMemoryDbHelper<ApplicationDbContext> _dbHelper = new(_seedFile, DbPersistence.OneDbPerTest);
    private static ApplicationDbContext _context = null!;
    private static UserManager<PopNGoUser> _userManager = null!;
    private static PopNGoUser _user = null!;

    [SetUp]
    public void Setup()
    {
        _context = _dbHelper.GetContext();

        _userManager = new(
            new UserStore<PopNGoUser>(_context),
            null,
            new PasswordHasher<PopNGoUser>(),
            new List<IUserValidator<PopNGoUser>>(),
            new List<IPasswordValidator<PopNGoUser>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new ServiceCollection().BuildServiceProvider(),
            new Logger<UserManager<PopNGoUser>>(new LoggerFactory())
        );

        _user = new PopNGoUser
        {
            UserName = "NewUser",
            Email = "NewUser@gmail.com",
            NotificationEmail = "NewUser@gmail.com",
            PhoneNumber = "123-456-7890",
            FirstName = "New",
            LastName = "User"
        };
    }

    [Test]
    public async Task ApplicationContext_NewUser_InvalidUsername_IsNotSuccessful()
    {
        // Arrange
        _user.UserName = "NewUser/";

        // Act
        await _userManager.CreateAsync(_user, "password");
        _userManager.UserValidators.Add(new UserValidator<PopNGoUser>());
        _userManager.PasswordValidators.Add(new PasswordValidator<PopNGoUser>());
        IdentityResult result = await _userManager.UserValidators.First().ValidateAsync(_userManager, _user);
        if(!result.Succeeded)
        {
            await _userManager.DeleteAsync(_user);
        }
        // Assert
        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task ApplicationContext_NewUser_IsSuccessful()
    {
        // Arrange
        _user.UserName = "NewUser";

        // Act
        IdentityResult result = await _userManager.CreateAsync(_user, "password1!");

        // Assert
        Assert.That(result.Succeeded, Is.True);
    }

    [Test]
    public async Task ApplicationContext_NewUser_IsInRole()
    {
        // Act
        await _userManager.CreateAsync(_user, "password1!");
        await _userManager.AddToRoleAsync(_user, "User");

        // Assert
        Assert.That(await _userManager.IsInRoleAsync(_user, "User"), Is.True);
    }

    [Test]
    public async Task ApplicationContext_NewUser_IsNotInRole()
    {
        // Act
        await _userManager.CreateAsync(_user, "password1!");

        // Assert
        Assert.That(await _userManager.IsInRoleAsync(_user, "User"), Is.False);
    }

    [Test]
    public async Task ApplicationContext_AdminUser_Seeded()
    {
        // Arrange
        _user.UserName = "admin@popngo.com";
        _user.Email = "popngo.wou@gmail.com";
        _user.EmailConfirmed = true;
        _user.FirstName = "The";
        _user.LastName = "Admin";

        // Act
        await _userManager.CreateAsync(_user, "Password");
        await _userManager.AddToRoleAsync(_user, "Admin");

        // Assert
        Assert.That(await _userManager.IsInRoleAsync(_user, "Admin"), Is.True);
    }
        
    [Test]
    public async Task ApplicationContext_ChangeNotificationEmail_IsSuccessful()
    {
        // Arrange
        string userId = "6b6c5d33-e6d7-4d18-a565-a657eaf7a8a5";
        PopNGoUser expected = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found");

        // Act
        expected.NotificationEmail = "JohnDoe@gmail.com";
        await _userManager.UpdateAsync(expected);

        // Assert
        Assert.That(_context.Users.Where(u => u.Id == userId).First().NotificationEmail
        , Is.EqualTo(expected.NotificationEmail));
    }

    [Test]
    public async Task ApplicationContext_NewUser_NotConfirmed()
    {
        // Act
        await _userManager.CreateAsync(_user, "Password123!");

        // Assert
        Assert.That(_context.Users.Where(u => u.UserName == "NewUser").First().EmailConfirmed, Is.False);

    }

    [Test]
    public void ApplicationContext_HasBeenSeeded()
    {
        Console.WriteLine("Users: " + _context.Users.Count());
        Assert.That(_context.Users.Count(), Is.EqualTo(4));
    }

   
}