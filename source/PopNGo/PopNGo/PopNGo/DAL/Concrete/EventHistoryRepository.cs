﻿using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.ExtensionMethods;

namespace PopNGo.DAL.Concrete
{
    public class EventHistoryRepository : Repository<EventHistory>, IEventHistoryRepository
    {
        private DbSet<EventHistory> _eventHistories;
        public EventHistoryRepository(ApplicationDbContext context) : base(context)
        {
            _eventHistories = context.EventHistories;
        }

        public List<PopNGo.Models.DTO.EventHistory> GetEventHistory(int userId)
        {
            return _eventHistories.Where(eh => eh.UserId == userId).Select(eh => eh.ToDTO()).ToList();
        }
    }
}
