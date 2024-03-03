using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using PopNGo.Models.DTO;


namespace PopNGo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class FavoritesApiController : Controller
{
    private readonly ILogger<FavoritesApiController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IFavoritesRepository _favoritesRepo;
    private readonly IEventRepository _eventRepo;
    private readonly UserManager<PopNGoUser> _userManager;
    private readonly IPgUserRepository _pgUserRepository;

    public FavoritesApiController(ILogger<FavoritesApiController> logger, IConfiguration configuration, IFavoritesRepository favoritesRepo, IEventRepository eventRepo, UserManager<PopNGoUser> userManager, IPgUserRepository pgUserRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _favoritesRepo = favoritesRepo;
        _eventRepo = eventRepo;
        _userManager = userManager;
        _pgUserRepository = pgUserRepository;
    }

    [HttpPost("AddFavorite")]
    public IActionResult AddFavorite([FromBody] PopNGo.Models.DTO.Event eventInfo) //AddFavoriteRequest is a class that contains a Favorite and an Event
    {
        PopNGoUser user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return Unauthorized();
        }

        PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
        if (pgUser == null)
        {
            return Unauthorized();
        }

        if (!_eventRepo.IsEvent(eventInfo.ApiEventID)) //If the event does not exist, add it to the events
        {
            _eventRepo.AddEvent(eventInfo.ApiEventID, eventInfo.EventDate, eventInfo.EventName, eventInfo.EventDescription, eventInfo.EventLocation);
        }

        if (_favoritesRepo.IsFavorite(pgUser.Id, eventInfo.ApiEventID)) //If the event is already a favorite, return
        {
             return Ok();
        }

        // Whether the event existed or not, add it to the favorites
        _favoritesRepo.AddFavorite(pgUser.Id, eventInfo.ApiEventID);
        return Ok();
    }

    [HttpPost("RemoveFavorite")]
    public IActionResult RemoveFavorite([FromBody] PopNGo.Models.DTO.Event eventInfo)
    {
        PopNGoUser user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return Unauthorized();
        }

        PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
        if (pgUser == null)
        {
            return Unauthorized();
        }

        if (!_eventRepo.IsEvent(eventInfo.ApiEventID)) //If the event does not exist, add it to the events
        {
            _eventRepo.AddEvent(eventInfo.ApiEventID, eventInfo.EventDate, eventInfo.EventName, eventInfo.EventDescription, eventInfo.EventLocation);
        }

        _favoritesRepo.RemoveFavorite(pgUser.Id, eventInfo.ApiEventID);
        return Ok();
    }

    [HttpGet("GetUserFavorites")]
    public ActionResult<IEnumerable<PopNGo.Models.DTO.Event>> GetUserFavorites() //works
    {
        PopNGoUser user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return Unauthorized();
        }

        PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
        if (pgUser == null)
        {
            return Unauthorized();
        }
        

        List<PopNGo.Models.DTO.Event> events = _favoritesRepo.GetUserFavorites(pgUser.Id);
        if (events == null || events.Count == 0)
        {
            return NotFound();
        }

        return events;
    }

    [HttpGet("IsFavorite")]
    public ActionResult<bool> IsFavorite(string eventId) //works
    {
        PopNGoUser user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return false; // Return false if the user is not authenticated, instead of returning Unauthorized which is misleading
        }

        PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
        if (pgUser == null)
        {
            return false; // Return false if the user is not found, instead of returning Unauthorized which is misleading
        }

        return _favoritesRepo.IsFavorite(pgUser.Id, eventId);
    }
}
