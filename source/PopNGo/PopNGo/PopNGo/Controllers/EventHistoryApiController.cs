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
public class EventHistoryApiController : Controller
{
    private readonly ILogger<EventHistoryApiController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEventHistoryRepository _eventHistoryRepository;
    private readonly IEventRepository _eventRepo;
    private readonly UserManager<PopNGoUser> _userManager;
    private readonly IPgUserRepository _pgUserRepository;

    public EventHistoryApiController(ILogger<EventHistoryApiController> logger, IConfiguration configuration, IEventHistoryRepository eventHistoryRepository, IEventRepository eventRepo, UserManager<PopNGoUser> userManager, IPgUserRepository pgUserRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _eventHistoryRepository = eventHistoryRepository;
        _eventRepo = eventRepo;
        _userManager = userManager;
        _pgUserRepository = pgUserRepository;
    }

    // GET: /api/EventHistoryApi/EventHistory
    [HttpGet("EventHistory")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Models.DTO.Event>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<Models.DTO.Event>> GetUserEventHistory()
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

        List<Models.DTO.Event> events = _eventHistoryRepository.GetEventHistory(pgUser.Id);
        if (events == null || events.Count == 0)
        {
            return NotFound();
        }

        return events;
    }

    // POST: /api/EventHistoryApi/EventHistory
    [HttpPost("EventHistory")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddEventToHistoryAsync([FromBody] PopNGo.Models.DTO.Event eventInfo)
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
            _eventRepo.AddEvent(eventInfo.ApiEventID, eventInfo.EventDate, eventInfo.EventName, eventInfo.EventDescription, eventInfo.EventLocation);
        }


        _eventHistoryRepository.AddEventHistory(pgUser.Id, eventInfo.ApiEventID);
        return Ok();
    }
}
