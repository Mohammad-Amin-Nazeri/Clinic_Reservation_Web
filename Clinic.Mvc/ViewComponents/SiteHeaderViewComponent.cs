using Microsoft.AspNetCore.Mvc;

namespace Clinic.Mvc.ViewComponents
{
    public class SiteHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteHeader");
        }
    }
}
