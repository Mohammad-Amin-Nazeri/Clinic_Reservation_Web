using Clinic.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Mvc.Controllers
{
    public class CaptchaController : Controller
    {
        public IActionResult Generate()
        {
            var img = CaptchaService.GenerateCaptchaImage(out string captchaText);

            // Store CAPTCHA text in session
            HttpContext.Session.SetString("CaptchaCode", captchaText);

            return File(img, "image/png");
        }

        public IActionResult ValidateCaptcha(string code)
        {
            var saved = HttpContext.Session.GetString("CaptchaCode");
            return Json(saved.Equals(code, StringComparison.OrdinalIgnoreCase));
        }
    }
}
