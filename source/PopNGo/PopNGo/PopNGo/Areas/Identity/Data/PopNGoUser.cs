using Microsoft.AspNetCore.Identity;

namespace PopNGo.Areas.Identity.Data;

public class PopNGoUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string LastName { get; set; }
}