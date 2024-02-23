using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;
using PopNGo.DAL.Abstract;
using PopNGo.DAL.Concrete;

using Microsoft.AspNetCore.Identity;
using PopNGo.Areas.Identity.Data;
// using PopNGo.Models.DTO;
// using PopNGo.ExtensionMethods;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/")]
public class EventApiController : Controller
{
    private readonly ILogger<EventApiController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEventHistoryRepository _eventHistoryRepository;
    private readonly IPgUserRepository _pgUserRepository;
    private readonly UserManager<PopNGoUser> _userManager;

    public EventApiController(IEventHistoryRepository eventHistoryRepository, IPgUserRepository pgUserRepository, UserManager<PopNGoUser> userManager, ILogger<EventApiController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _eventHistoryRepository = eventHistoryRepository;
        _pgUserRepository = pgUserRepository;
        _userManager = userManager;

    }

    // GET: api/eventHistory
    [HttpGet("eventHistory")]
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
}
