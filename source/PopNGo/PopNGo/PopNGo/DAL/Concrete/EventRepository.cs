using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.ExtensionMethods;

namespace PopNGo.DAL.Concrete
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly DbSet<Event> _event;
        public EventRepository(PopNGoDB context) : base(context)
        {
            _event = context.Events;
        }

        public void AddEvent(string EventId, DateTime EventDate, string EventName, string EventDescription, string EventLocation, string EventImage)
        {
            ValidateEventParameters(EventId, EventDate, EventName, EventDescription, EventLocation);
            var newEvent = new Event { 
                ApiEventId = EventId, 
                EventDate = EventDate, 
                EventName = EventName, 
                EventDescription = EventDescription, 
                EventLocation = EventLocation,
                EventImage = EventImage
            };
            AddOrUpdate(newEvent);
        }

        public bool IsEvent(string apiEventId)
        {
            if (string.IsNullOrEmpty(apiEventId))
            {
                throw new ArgumentException("ApiEventId cannot be null or empty", nameof(apiEventId));
            }
            return _event.Any(e => e.ApiEventId == apiEventId);
        }

        private void ValidateEventParameters(string eventId, DateTime eventDate, string eventName, string eventDescription, string eventLocation)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                throw new ArgumentException("EventId cannot be null or empty", nameof(eventId));
            }

            if (eventDate == default(DateTime))
            {
                throw new ArgumentException("EventDate cannot be default", nameof(eventDate));
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException("EventName cannot be null or empty", nameof(eventName));
            }

            if (string.IsNullOrEmpty(eventDescription))
            {
                throw new ArgumentException("EventDescription cannot be null or empty", nameof(eventDescription));
            }

            if (string.IsNullOrEmpty(eventLocation))
            {
                throw new ArgumentException("EventLocation cannot be null or empty", nameof(eventLocation));
            }
        }
    }
}
