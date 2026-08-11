using Clinic.Application.Services.Interfaces;
using Clinic.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Mvc.ViewComponents
{
    public class SiteNevViewComponent(IUserService userService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await userService.GetUserById(User.GetUserId());
            return View("SiteNevViewComponent" , user);
        }
    }
}
