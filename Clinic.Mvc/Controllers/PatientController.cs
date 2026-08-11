using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Patients;
using Clinic.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Mvc.Controllers
{
    public class PatientController(IUserService userService) : SiteBaseController
    {
        #region Filter
        [HttpGet("search-patients")]
        public async Task<IActionResult> SearchPatient(FilterPatientsDto filter)
        {
            filter.TakeEntity = 4;
            var data = await userService.FilterPatients(filter);
            return View(data);
        }
        #endregion

        #region Create Group
        [HttpGet("create-patients")]
        public IActionResult CreateGroupPatient()
        {
            return View();
        }

        [HttpPost("create-patients")]
        public async Task<IActionResult> CreateGroupPatient(List<CreateGroupPatientsDto> patients)
        {
            var res = await userService.CreateGroupPatients(patients);

            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    var errors = res.Items != null && res.Items.Any() ? string.Join(" - ", res.Items) : string.Empty;
                    TempData[ErrorMessage] = $"{res.Message} {(string.IsNullOrEmpty(errors) ? "" : $" - {errors}")}";
                    return View(patients);
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("SearchPatient");
                case ResultStatus.Warning:
                    var warnings = res.Items != null && res.Items.Any() ? string.Join(" - ", res.Items) : string.Empty;
                    TempData[WarningMessage] = $"{res.Message} {(string.IsNullOrEmpty(warnings) ? "" : $" - {warnings}")}";
                    return RedirectToAction("SearchPatient");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Patient Detail
        public async Task<IActionResult> PatientDetail(int id)
        {
            var data = await userService.PatientDetail(id);
            return View(data);
        }
        #endregion

        #region Create Single
        [HttpGet("create-patient")]
        public IActionResult CreateSinglePatient()
        {
            return View();
        }

        [HttpPost("create-patient")]
        public async Task<IActionResult> CreateSinglePatient(CreatePatientDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            var res = await userService.CreatePatient(dto);

            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    var errors = res.Items != null && res.Items.Any() ? string.Join(" - ", res.Items) : string.Empty;
                    TempData[ErrorMessage] = $"{res.Message} {(string.IsNullOrEmpty(errors) ? "" : $" - {errors}")}";

                    return View(dto);
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("SearchPatient");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Edit
        [HttpGet("edit-patient")]
        public async Task<IActionResult> EditPatient(int id)
        {
            var data = await userService.GetEditPatient(id);
            return View(data);
        }

        [HttpPost("edit-patient")]
        public async Task<IActionResult> EditPatient(EditPatientDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            var res = await userService.EditPatient(dto);
            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    TempData[ErrorMessage] = res.Message;
                    return View(dto);
                    break;
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("SearchPatient");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Delete
        [Route("delete-Patient/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var res = await userService.DeletePatient(id);
            TempData[InfoMessage] = res.Message;
            return RedirectToAction("SearchPatient");
        }
        #endregion
    }
}
