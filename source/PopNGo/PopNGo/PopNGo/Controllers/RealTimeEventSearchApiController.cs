using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;
using PopNGo.Services;

namespace PopNGo.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RealTimeEventSearchApiController : ControllerBase
    {
        private readonly IRealTimeEventSearchService _realTimeEventSearchService;
        private readonly ILogger<RealTimeEventSearchApiController> _logger;
        public RealTimeEventSearchApiController(IRealTimeEventSearchService realTimeEventSearchService, ILogger<RealTimeEventSearchApiController> logger)
        {
            _realTimeEventSearchService = realTimeEventSearchService;
            _logger = logger;
        }
        // GET: api/search/events/?q=Q&start=0
        [HttpGet("search/events")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventDetail>))]
        public async Task<ActionResult<IEnumerable<EventDetail>>> SearchMoviesAsync(string q, int start)
        {
            try
            {
                IEnumerable<EventDetail> events = await _realTimeEventSearchService.SearchEventAsync(q, start);
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching movies repositories");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }

}
