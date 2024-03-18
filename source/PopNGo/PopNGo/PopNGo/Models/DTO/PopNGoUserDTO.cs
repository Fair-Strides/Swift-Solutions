namespace PopNGo.Models.DTO
{
    public class PopNGoUserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NotificationEmail { get; set; }
        public bool NotifyDayBefore { get; set; }
        public bool NotifyDayOf { get; set; }
        public bool NotifyWeekBefore { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, UserName: {UserName}, FirstName: {FirstName}, LastName: {LastName}, Email: {Email}, NotificationEmail: {NotificationEmail}, NotifyDayBefore: {NotifyDayBefore}, NotifyDayOf: {NotifyDayOf}, NotifyWeekBefore: {NotifyWeekBefore}";
        }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class PopNGoUserExtensions
    {
        public static Models.DTO.PopNGoUserDTO ToDTO(this Areas.Identity.Data.PopNGoUser User)
        {
            return new Models.DTO.PopNGoUserDTO
            {
                Id = User.Id,
                UserName = User.UserName,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                NotificationEmail = User.NotificationEmail,
                NotifyDayBefore = User.NotifyDayBefore,
                NotifyDayOf = User.NotifyDayOf,
                NotifyWeekBefore = User.NotifyWeekBefore
            };
        }
    }
}