using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.DAL.Concrete
{
    public class FavoritesRepository : Repository<FavoriteEvent>, IFavoritesRepository
    {
        private readonly DbSet<FavoriteEvent> _favoriteEvents;
        private readonly DbSet<Event> _events;

        public FavoritesRepository(PopNGoDB context) : base(context)
        {
            _favoriteEvents = context.FavoriteEvents;
            _events = context.Events;
        }

        public void AddFavorite(int userId, string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                throw new ArgumentException("EventId cannot be null or empty", nameof(eventId));
            }

            var eventEntity = _events.FirstOrDefault(e => e.ApiEventId == eventId);
            if (eventEntity == null)
            {
                throw new ArgumentException($"No event found with the id {eventId}", nameof(eventId));
            }
            var favoriteEvent = new FavoriteEvent { UserId = userId, EventId = eventEntity.Id };

            try
            {
                AddOrUpdate(favoriteEvent);
            }
            catch (Exception ex)
            {
                // Log the exception, rethrow it, or handle it in some other way
                throw new Exception("Error adding or updating favorite event", ex);
            }
        }

        public void RemoveFavorite(int userId, string apiEventId)
        {
            if (string.IsNullOrEmpty(apiEventId))
            {
                throw new ArgumentException("ApiEventId cannot be null or empty", nameof(apiEventId));
            }

            var favoriteEvent = _favoriteEvents.FirstOrDefault(fe => fe.UserId == userId && fe.Event.ApiEventId == apiEventId);
            if (favoriteEvent == null)
            {
                throw new ArgumentException($"No favorite event found for user {userId} with the id {apiEventId}", nameof(apiEventId));
            }

            try
            {
                Delete(favoriteEvent);
            }
            catch (Exception ex)
            {
                // Log the exception, rethrow it, or handle it in some other way
                throw new Exception("Error deleting favorite event", ex);
            }
        }

        public List<PopNGo.Models.DTO.Event> GetUserFavorites(int userId)
        {
            List<PopNGo.Models.DTO.Event> userFavorites;
            try
            {
                userFavorites = _favoriteEvents
                    .Where(fe => fe.UserId == userId)
                    .Select(fe => fe.Event)
                    .Select(e => new PopNGo.Models.DTO.Event
                    {
                        // Assuming PopNGo.Models.DTO.Event and PopNGo.Models.Event have similar properties
                        ApiEventID = e.ApiEventId,
                        EventDate = e.EventDate,
                        EventName = e.EventName,
                        EventDescription = e.EventDescription,
                        EventLocation = e.EventLocation
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log the exception, rethrow it, or handle it in some other way
                throw new Exception("Error retrieving user favorites", ex);
            }

            return userFavorites;
        }

        public bool IsFavorite(int userId, string apiEventId)
        {
            if (string.IsNullOrEmpty(apiEventId))
            {
                throw new ArgumentException("ApiEventId cannot be null or empty", nameof(apiEventId));
            }

            bool isFavorite;
            try
            {
                isFavorite = _favoriteEvents.Any(fe => fe.UserId == userId && fe.Event.ApiEventId == apiEventId);
            }
            catch (Exception ex)
            {
                // Log the exception, rethrow it, or handle it in some other way
                throw new Exception("Error checking if event is a favorite", ex);
            }

            return isFavorite;
        }
    }
}
