using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;

namespace PopNGo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult WelcomePage()
    {
        return View();
    }
    public IActionResult ExplorePage()
    {
        return View();
    }
    
    public IActionResult HistoryPage()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
