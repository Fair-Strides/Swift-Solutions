using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;

namespace PopNGo.Controllers;

public class MapController : Controller
{
    private readonly ILogger<MapController> _logger;

    public MapController(ILogger<MapController> logger)
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
