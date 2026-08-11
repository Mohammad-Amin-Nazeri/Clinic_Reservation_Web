using System.Globalization;
using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Reservations;
using Clinic.Application.Services.Interfaces;
using Clinic.Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Mvc.Controllers
{
    public class ReservationController(IReservationService reservationService) : SiteBaseController
    {
        #region Filter
        [HttpGet("search-days")]
        [HttpGet("search-days/{year}-{month}")]
        public IActionResult SearchDays(int? year, int? month)
        {
            var pc = new PersianCalendar();
            var now = DateTime.Now;

            // Check for year and month in url (method input)
            if (year == null || year <= 0)
                year = pc.GetYear(now);

            if (month == null || month <= 0 || month > 12)
                month = pc.GetMonth(now);

            // Validation
            if (year < 1 || year > 9999 || month < 1 || month > 12)
            {
                TempData[ErrorMessage] = "ماه یا سال نامعتبر است.";
                return RedirectToAction("SearchDays");
            }

            // Send year and month to view via view bag
            ViewBag.SelectedYear = year;
            ViewBag.SelectedMonth = month;

            // Calculate current selected month days
            var daysInMonth = pc.GetDaysInMonth(year.Value, month.Value);
            var startDate = pc.ToDateTime(year.Value, month.Value, 1, 0, 0, 0, 0);

            var days = Enumerable.Range(0, daysInMonth)
                .Select(i => startDate.AddDays(i))
                .ToList();

            return View(days);
        }
        #endregion

        #region Day Reservations
        [HttpGet("date-reservations/{date}")]
        public async Task<IActionResult> DateReservations(DateTime date)
        {
            var filter = new FilterReservationsDto
            {
                ReserveDate = date.ToMiladi(),
                TakeEntity = 30
            };
            var data = await reservationService.FilterReservation(filter);

            return View(data);
        }
        #endregion

        #region Create Group
        [HttpGet("create-reservation")]
        public IActionResult CreateGroupReservation()
        {
            return View();
        }

        [HttpPost("create-reservation")]
        public async Task<IActionResult> CreateGroupReservation(CreateGroupReservationDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            var res = await reservationService.CreateGroupReservations(dto);

            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    TempData[ErrorMessage] = res.Message;
                    return View(dto);
                    break;
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("SearchDays");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Create Single
        [HttpGet("create-single")]
        public IActionResult CreateSingleReservation()
        {
            return View();
        }

        [HttpPost("create-single")]
        public async Task<IActionResult> CreateSingleReservation(CreateReservationDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            var res = await reservationService.CreateReservations(dto);

            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    TempData[ErrorMessage] = res.Message;
                    return View(dto);
                    break;
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("SearchDays");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Delete
        [Route("delete-reservation/{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var res = await reservationService.DeleteReservation(id);
            TempData[InfoMessage] = res.Message;
            return RedirectToAction("SearchDays");
        }
        #endregion

        #region Delete Group Reservations
        [Route("delete-group-reservations")]
        public async Task<IActionResult> DeleteGroupReservation(int year, int month)
        {
            var res = await reservationService.DeleteGroupReservation(year, month);
            TempData[SuccessMessage] = res.Message;
            return RedirectToAction("SearchDays");
        }
        #endregion
    }
}
