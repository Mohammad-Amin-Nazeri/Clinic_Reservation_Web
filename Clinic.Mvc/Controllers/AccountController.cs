using Clinic.Application.DTOs.Users;
using Clinic.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Clinic.Application.DTOs.Common;

namespace Clinic.Mvc.Controllers
{
    public class AccountController(IUserService userService, IOtpService otpService) : SiteBaseController
    {
        #region Login
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                ViewData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            #region Can Receive OTP
            if (!otpService.CanRegenrateOtp(dto.Mobile))
            {
                ViewData[ErrorMessage] = "نمیتوانید تا 2 دقیقه درخواست کد اعتبارسنجی بدهید.";
                return View();
            }
            #endregion

            #region Check Captcha
            var savedCaptcha = HttpContext.Session.GetString("CaptchaCode");
            if (!string.Equals(dto.CaptchaCode, savedCaptcha, StringComparison.OrdinalIgnoreCase))
            {
                ViewData[ErrorMessage] = "لطفا اعداد و حروف تصویر را به درستی وارد کنید.";
                return View();
            }
            #endregion

            var res = await userService.Login(dto);

            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    TempData[ErrorMessage] = res.Message;
                    return View();
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("Authentication", new { mobile = dto.Mobile });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Authentication
        [HttpGet("authentication")]
        public IActionResult Authentication(string mobile)
        {
            var model = new AuthenticationDto { Mobile = mobile };
            return View(model);
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Authentication(AuthenticationDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            // Check OTP
            var res = await userService.CheckOtp(dto);
            if (res.ResultStatus == ResultStatus.Error)
            {
                TempData[ErrorMessage] = res.Message;
                return RedirectToAction("Login");
            }

            // Create Claims
            var user = await userService.GetUserByMobile(dto.Mobile);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name , user.Mobile),
                new(ClaimTypes.NameIdentifier , user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(principal, properties);

            TempData[SuccessMessage] = "خوش آمدید!";
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Resend OTP
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto request)
        {
            if (string.IsNullOrEmpty(request.Mobile))
                return BadRequest(new { message = "شماره موبایل معتبر نیست." });

            if (!otpService.CanRegenrateOtp(request.Mobile))
                return Ok(new { message = "لطفا پس از 2 دقیقه برای دریافت مجدد کد اقدام کنید." });

            await userService.ResendOtp(request.Mobile);
            return Ok(new { message = "کد اعتبارسنجی ارسال شد." });
        }
        #endregion

        #region Log Out
        [Route("log-out")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            TempData[InfoMessage] = "از حساب کاربری خود خارج شده اید.";
            return RedirectToAction("Login");
        }
        #endregion
    }
}
