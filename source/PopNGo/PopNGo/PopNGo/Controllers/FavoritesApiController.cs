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
    public async Task<IActionResult> AddFavorite([FromBody] PopNGo.Models.DTO.Event eventInfo) //AddFavoriteRequest is a class that contains a Favorite and an Event
    {
        try
        {
            PopNGoUser user = await _userManager.GetUserAsync(User);
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
                _eventRepo.AddEvent(eventInfo.ApiEventID, eventInfo.EventDate, eventInfo.EventName, eventInfo.EventDescription, eventInfo.EventLocation, eventInfo.EventImage);
            }

            // Whether the event existed or not, add it to the favorites
            _favoritesRepo.AddFavorite(pgUser.Id, eventInfo.ApiEventID); // AddFavorite already has error handling
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception, for example:
            _logger.LogError(ex, "An error occurred while adding a favorite event.");

            // Return a 500 Internal Server Error status code and a message to the client
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("RemoveFavorite")]
    public async Task<IActionResult> RemoveFavorite([FromBody] PopNGo.Models.DTO.Event eventInfo)
    {
        PopNGoUser user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
        if (pgUser == null)
        {
            return Unauthorized();
        }

        try
        {
            if (!_eventRepo.IsEvent(eventInfo.ApiEventID)) //If the event does not exist, add it to the events
            {
                _eventRepo.AddEvent(eventInfo.ApiEventID, eventInfo.EventDate, eventInfo.EventName, eventInfo.EventDescription, eventInfo.EventLocation, eventInfo.EventImage);
            }

            _favoritesRepo.RemoveFavorite(pgUser.Id, eventInfo.ApiEventID);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception, for example:
            _logger.LogError(ex, "An error occurred while removing a favorite event.");

            // Return a 500 Internal Server Error status code and a message to the client
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("GetUserFavorites")]
    public async Task<ActionResult<IEnumerable<PopNGo.Models.DTO.Event>>> GetUserFavorites()
    {
        try
        {
            PopNGoUser user = await _userManager.GetUserAsync(User);
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
        catch (Exception ex)
        {
            // Log the exception, for example:
            _logger.LogError(ex, "An error occurred while getting user favorites.");

            // Return a 500 Internal Server Error status code and a message to the client
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("IsFavorite")]
    public async Task<ActionResult<bool>> IsFavorite(string eventId)
    {
        try
        {
            PopNGoUser user = await _userManager.GetUserAsync(User);
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
        catch (Exception ex)
        {
            // Log the exception, for example:
            _logger.LogError(ex, "An error occurred while checking if the event is a favorite.");

            // Return a 500 Internal Server Error status code and a message to the client
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
