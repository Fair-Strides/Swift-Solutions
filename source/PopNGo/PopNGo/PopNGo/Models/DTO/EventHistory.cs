using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PopNGo.Models.DTO
{
    public class EventHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ViewedDate { get; set; }
        public string EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventLocation { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class EventHistoryExtensions
    {
        public static PopNGo.Models.DTO.EventHistory ToDTO(this PopNGo.Models.EventHistory eventHistory)
        {
            return new PopNGo.Models.DTO.EventHistory
            {
                Id = eventHistory.Id,
                UserId = eventHistory.UserId,
                ViewedDate = eventHistory.ViewedDate,
                EventId = eventHistory.EventId,
                EventDate = eventHistory.EventDate,
                EventDescription = eventHistory.EventDescription,
                EventLocation = eventHistory.EventLocation,
                EventName = eventHistory.EventName
    };
        }
    }
}