using Clinic.Application.DTOs.ReserveRecords;
using Clinic.Application.Services.Interfaces;
using Clinic.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Mvc.Controllers
{
    public class HomeController(IRecordService recordService) : SiteBaseController
    {
        public async Task<IActionResult> Index()
        {
            var userId = User.GetUserId();
            if (userId == 0)
            {
                return RedirectToAction("Login" , "Account");
            }

            var filter = new FilterRecordsDto
            {
                ReservationDate = DateTime.Now.Date.AddDays(3),
                TakeEntity = 10
            };
            var data = await recordService.FilterRecords(filter);
            return View(data);
        }
    }
}
