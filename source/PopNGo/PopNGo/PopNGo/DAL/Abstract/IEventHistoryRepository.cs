using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IEventHistoryRepository : IRepository<EventHistory>
    {
        public List<PopNGo.Models.DTO.EventHistory> GetEventHistory(int userId);
    }
}
