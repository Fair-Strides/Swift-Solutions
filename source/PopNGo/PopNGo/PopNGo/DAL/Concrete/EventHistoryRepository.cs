using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.ExtensionMethods;

namespace PopNGo.DAL.Concrete
{
    public class EventHistoryRepository : Repository<EventHistory>, IEventHistoryRepository
    {
        private readonly DbSet<EventHistory> _eventHistories;
        public EventHistoryRepository(PopNGoDB context) : base(context)
        {
            _eventHistories = context.EventHistories;
        }

        public List<PopNGo.Models.DTO.Event> GetEventHistory(int userId)
        {
            return _eventHistories.Where(eh => eh.UserId == userId).Select(eh => eh.Event.ToDTO()).ToList();
        }
    }
}
