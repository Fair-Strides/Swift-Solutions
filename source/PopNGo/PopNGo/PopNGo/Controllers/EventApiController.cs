using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;
using PopNGo.DAL.Abstract;
using PopNGo.DAL.Concrete;

using Microsoft.AspNetCore.Identity;
using PopNGo.Areas.Identity.Data;
// using PopNGo.Models.DTO;
// using PopNGo.ExtensionMethods;
using PopNGo.Models.DTO;
using PopNGo.ExtensionMethods;

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
<<<<<<< HEAD
    private readonly ITagRepository _tagRepository;

    public EventApiController(
        IEventHistoryRepository eventHistoryRepository,
        IPgUserRepository pgUserRepository,
        UserManager<PopNGoUser> userManager,
        ILogger<EventApiController> logger,
        IConfiguration configuration,
        ITagRepository tagRepository
    )
=======

    public EventApiController(IEventHistoryRepository eventHistoryRepository, IPgUserRepository pgUserRepository, UserManager<PopNGoUser> userManager, ILogger<EventApiController> logger, IConfiguration configuration)
>>>>>>> main
    {
        _logger = logger;
        _configuration = configuration;
        _eventHistoryRepository = eventHistoryRepository;
        _pgUserRepository = pgUserRepository;
        _userManager = userManager;
<<<<<<< HEAD
        _tagRepository = tagRepository;

    }

    [HttpGet("tags/name={tag}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.DTO.Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.DTO.Tag>> TagExists(string tag)
    {
        Models.Tag foundTag = await _tagRepository.FindByName(tag);
        foundTag ??= (await _tagRepository.CreateNew(tag));

        return foundTag.ToDTO();
    }

    [HttpPost("tags/create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> CreateTags([FromBody] string[] tags)
    {
        foreach (string tag in tags)
        {
            // if(tag.Length > 255)
            // {
                //return BadRequest($"Tag {tag} is too long");
            // }

            // Skip any tags that are too long
            if(tag.Length <= 255) {
                Models.Tag foundTag = await _tagRepository.FindByName(tag);
                foundTag ??= (await _tagRepository.CreateNew(tag));
            }
        }

        return true;
=======

    }

    // GET: api/eventHistory
    [HttpGet("eventHistory")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Models.DTO.EventHistory>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<Models.DTO.EventHistory>> GetUserEventHistory()
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

        List<Models.DTO.EventHistory> events = _eventHistoryRepository.GetEventHistory(pgUser.Id);
        if (events == null || events.Count == 0)
        {
            return NotFound();
        }

        return events;
>>>>>>> main
    }
}
