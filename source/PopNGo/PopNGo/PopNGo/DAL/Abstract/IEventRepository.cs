using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IEventRepository : IRepository<Event>
    {
        // Add methods specific to Event here
        void AddEvent(string EventId, DateTime EventDate, string EventName, string EventDescription, string EventLocation, string EventImage);
        
        bool IsEvent(string eventId);
        
    }
}
