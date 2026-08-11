using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Users;
using Clinic.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Mvc.Controllers
{
    public class AdminController(IUserService userService) : SiteBaseController
    {
        #region List
        [HttpGet("users-list")]
        public async Task<IActionResult> AdminsList()
        {
            var data = await userService.GetUsers();
            return View(data);
        }
        #endregion

        #region Create 
        [HttpGet("create-user")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            var res = await userService.CreateUser(dto);

            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    TempData[ErrorMessage] = res.Message;
                    return View(dto);
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("AdminsList");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Edit
        [HttpGet("update-user")]
        public async Task<IActionResult> EditUser(int id)
        {
            var data = await userService.GetEditUser(id);
            return View(data);
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> EditUser(EditUserDto dto)
        {
            #region Chech ModelState
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفا فرم را با دقت پر کنید.";
                return View(dto);
            }
            #endregion

            var res = await userService.EditUser(dto);

            switch (res.ResultStatus)
            {
                case ResultStatus.Error:
                    TempData[ErrorMessage] = res.Message;
                    return View(dto);
                    break;
                case ResultStatus.Success:
                    TempData[SuccessMessage] = res.Message;
                    return RedirectToAction("AdminsList");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Delete
        [Route("delete-admin/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            await userService.DeleteUser(id);
            return RedirectToAction("AdminsList");
        }
        #endregion
    }
}
