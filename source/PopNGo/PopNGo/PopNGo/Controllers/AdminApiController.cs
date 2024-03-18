using Microsoft.AspNetCore.Mvc;
using PopNGo.DAL.Abstract;

using Microsoft.AspNetCore.Identity;
using PopNGo.Areas.Identity.Data;
using PopNGo.ExtensionMethods;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/admin/")]
public class AdminApiController : Controller
{
    private readonly ILogger<EventApiController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IPgUserRepository _pgUserRepository;
    private readonly UserManager<PopNGoUser> _userManager;
    private readonly IEmailSender _emailSender;

    public AdminApiController(
        IPgUserRepository pgUserRepository,
        UserManager<PopNGoUser> userManager,
        ILogger<EventApiController> logger,
        IConfiguration configuration,
        IEmailSender emailSender
    )
    {
        _logger = logger;
        _configuration = configuration;
        _pgUserRepository = pgUserRepository;
        _userManager = userManager;
        _emailSender = emailSender;

    }

    [HttpGet("users/sendVerificationEmail/userId={userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> ResendVerificationEmail(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }
        try {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            
            callbackUrl = callbackUrl.Replace($"AdminApi/ResendVerificationEmail", "Identity/Account/ConfirmEmail");
            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        } catch (Exception e) {
            _logger.LogError(e, "Failed to send verification email to user {userId}", userId);
            return false;
        }
        return true;
    }

    [HttpGet("users/sendPasswordResetEmail/userId={userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> ResendPasswordResetEmail(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        try {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            
            callbackUrl = callbackUrl.Replace($"AdminApi/ResendPasswordResetEmail", "Identity/Account/ResetPassword");
            await _emailSender.SendEmailAsync(
                user.Email,
                "Reset your password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        } catch (Exception e) {
            _logger.LogError(e, "Failed to send password reset email to user {userId}", userId);
            return false;
        }
        return true;
    }

    [HttpGet("users/confirmUserAccount/userId={userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> ConfirmUserAccount(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        try {
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        } catch (Exception e) {
            _logger.LogError(e, "Failed to confirm user account for user {userId}", userId);
            return false;
        }
        return true;
    }

    [HttpGet("users/getUserInfo/userId={userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.DTO.PopNGoUserDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.DTO.PopNGoUserDTO>> GetUserInfo(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        return user.ToDTO();
    }

    [HttpPost("users/updateUserAccount/userId={userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> UpdateUserAccount(string userId, Models.DTO.PopNGoUserDTO userData)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        try {
            user.Email = userData.Email;
            user.NormalizedEmail = userData.Email.ToUpper();
            user.FirstName = userData.FirstName;
            user.LastName = userData.LastName;
            user.NormalizedUserName = userData.UserName.ToUpper();
            user.UserName = userData.UserName;
            user.NotificationEmail = userData.NotificationEmail;
            user.NotifyDayBefore = userData.NotifyDayBefore;
            user.NotifyDayOf = userData.NotifyDayOf;
            user.NotifyWeekBefore = userData.NotifyWeekBefore;

            await _userManager.UpdateAsync(user);
        } catch (Exception e) {
            _logger.LogError(e, "Failed to update user account for user {userId}", userId);
            return false;
        }
        return true;
    }

    [HttpDelete("users/deleteUserAccount/userId={userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteUserAccount(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        try {
            await _userManager.DeleteAsync(user);
        } catch (Exception e) {
            _logger.LogError(e, "Failed to delete user account for user {userId}", userId);
            return false;
        }
        return true;
    }
}