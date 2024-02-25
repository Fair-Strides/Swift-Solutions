using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;

namespace PopNGo.Controllers;

public class FavoritesController : Controller
{
    private readonly ILogger<FavoritesController> _logger;

    public FavoritesController(ILogger<FavoritesController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
