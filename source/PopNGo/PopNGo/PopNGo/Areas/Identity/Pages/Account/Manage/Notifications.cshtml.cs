// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PopNGo.Areas.Identity.Data;

namespace PopNGo.Areas.Identity.Pages.Account.Manage
{
    public class NotificationsModel : PageModel
    {
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly SignInManager<PopNGoUser> _signInManager;

        public NotificationsModel(
            UserManager<PopNGoUser> userManager,
            SignInManager<PopNGoUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Display(Name = "Current email")]
        public string NotificationEmail { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///    This property is used to specify a user's notification email display purposes.
            ///    This property is required.
            /// </summary>
            [Required]
            [Display(Name = "New email")]
            public string NotificationEmail { get; set; }
        }

        private void Load(PopNGoUser user)
        {
            var notificationEmail = user.NotificationEmail;
            NotificationEmail = notificationEmail;

            Input = new InputModel
            {
                NotificationEmail = notificationEmail
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            var notificationEmail = user.NotificationEmail;

            // Only update the user if there are changes
            if (notificationEmail != Input.NotificationEmail)
            {
                user.NotificationEmail = Input.NotificationEmail;
                await _userManager.UpdateAsync(user);
            // var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            // if (Input.PhoneNumber != phoneNumber)
            // {
            //     var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //     if (!setPhoneResult.Succeeded)
            //     {
            //         StatusMessage = "Unexpected error when trying to set phone number.";
            //         return RedirectToPage();
            //     }
            // }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
