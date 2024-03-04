using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IEventHistoryRepository : IRepository<EventHistory>
    {
        public List<PopNGo.Models.DTO.Event> GetEventHistory(int userId);
        public void AddEventHistory(int userId, string apiEventId);
    }
}
