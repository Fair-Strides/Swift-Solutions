using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;
// using PopNGo.DAL.Abstract;
// using PopNGo.DAL.Concrete;
// using PopNGo.Models.DTO;
// using PopNGo.ExtensionMethods;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MapApiController : Controller
{
    private readonly ILogger<MapApiController> _logger;
    private readonly IConfiguration _configuration;
    private string _googleApiKey;
    // private readonly "REPOSITORIES"

    public MapApiController(ILogger<MapApiController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _googleApiKey = _configuration["GoogleMapsApiKey"];
        // _repo = new "REPOSITORIES"
    }

    [HttpGet("GetApiKey")]
    public IActionResult GetApiKey()
    {
        return Ok(_googleApiKey);
    }
}
