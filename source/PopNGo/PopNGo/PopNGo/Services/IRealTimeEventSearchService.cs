using PopNGo.Models;

namespace PopNGo.Services
{
    public interface IRealTimeEventSearchService
    {
        Task<IEnumerable<EventDetail>> SearchEventAsync(string query, int start);
    }
}
