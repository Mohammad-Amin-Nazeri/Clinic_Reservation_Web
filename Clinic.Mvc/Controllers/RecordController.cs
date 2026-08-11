using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.ReserveRecords;
using Clinic.Application.Services.Interfaces;
using Clinic.Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Mvc.Controllers
{
    public class RecordController(IRecordService recordService , IReservationService reservationService) : SiteBaseController
    {
        #region Filter
        [HttpGet("filter-record")]
        public async Task<IActionResult> FilterRecord(FilterRecordsDto filter , string? date)
        {
            if (date != null)
            {
                var dateTime = date.ConvertShamsiStringToDateTime();
                var miladiDate = dateTime.ToMiladi();
                filter.ReservationDate = miladiDate;
            }
            var data = await recordService.FilterRecords(filter);

            return View(data);
        }
        #endregion

        #region Detail
        [HttpGet("record-detail/{id}")]
        public async Task<IActionResult> RecordDetail(int id)
        {
            var data = await recordService.ReservationRecordDetail(id);
            return View(data);
        }
        #endregion

        #region Create 
        [HttpPost("create-record")]
        public async Task<IActionResult> CreateRecord(ReserveTimeDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            var res = await recordService.SubmitReservation(dto);

            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    var reservation = await reservationService.GetReservationById(dto.ReservationId);
                    TempData[ErrorMessage] = res.Message;
                    return RedirectToAction("DateReservations", "Reservation" , new {reservation.ReserveDate.Date});
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("FilterRecord");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Edit 
        [HttpGet("edit-record")]
        public async Task<IActionResult> EditRecord(int id)
        {
            var data = await recordService.GetEditRecord(id);
            return View(data);
        }

        [HttpPost("edit-record")]
        public async Task<IActionResult> EditRecord(EditRecordDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            var res = await recordService.UpdateRecord(dto);

            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    TempData[ErrorMessage] = res.Message;
                    return View(dto);
                    break;
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("FilterRecord");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Delete
        [Route("delete-record/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            await recordService.DeleteRecord(id);
            TempData[SuccessMessage] = "عملیات با موفقیت انجام شد.";
            return RedirectToAction("FilterRecord");
        }
        #endregion
    }
}
