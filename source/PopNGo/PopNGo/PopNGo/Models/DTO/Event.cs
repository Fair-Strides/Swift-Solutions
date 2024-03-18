using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PopNGo.Models.DTO
{
    public class Event
    {
        public int Id { get; set; }
        public string ApiEventID { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventLocation { get; set; }

        public string EventImage { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class EventExtensions
    {
        public static PopNGo.Models.DTO.Event ToDTO(this PopNGo.Models.Event Event)
        {
            return new PopNGo.Models.DTO.Event
            {
                Id = Event.Id,
                ApiEventID = Event.ApiEventId,
                EventDate = Event.EventDate,
                EventDescription = Event.EventDescription,
                EventLocation = Event.EventLocation,
                EventName = Event.EventName,
                EventImage = Event.EventImage
            };
        }
    }
}