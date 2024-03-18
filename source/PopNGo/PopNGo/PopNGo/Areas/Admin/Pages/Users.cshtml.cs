// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PopNGo.Areas.Identity.Data;

namespace PopNGo.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<PopNGoUser> _userManager;

        public UsersModel(UserManager<PopNGoUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public List<UserModel> Users { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class UserModel
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string NotificationEmail { get; set; }
            public List<string> Roles { get; set; }
            public string Id { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            StatusMessage = "You are not authorized to view this page.";
            
            if(User.Identity.IsAuthenticated)
            {
                StatusMessage = "";

                Users = new List<UserModel>();
                foreach (var user in _userManager.Users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    Users.Add(new UserModel
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        NotificationEmail = user.NotificationEmail,
                        Roles = roles.ToList(),
                        Id = user.Id
                    });
                }            

            }

            return Page();
        }
    }
}