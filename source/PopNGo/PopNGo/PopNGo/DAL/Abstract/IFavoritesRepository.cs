using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IFavoritesRepository : IRepository<FavoriteEvent>
    {
        public void AddFavorite(int userId, string eventId);
        public void RemoveFavorite(int userId, string eventId);
        public List<PopNGo.Models.DTO.Event> GetUserFavorites(int userId);
        public bool IsFavorite(int userId, string eventId);
    }
}
