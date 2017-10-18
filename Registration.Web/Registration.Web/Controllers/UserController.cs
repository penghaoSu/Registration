using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Registration.Service.Models;
using Registration.Web.Models;
using Microsoft.AspNetCore.Identity;
using Registration.Service.Interface;
using Registration.Data.Enums;
using Registration.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace Registration.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IUserTokenService _userTokenService;
        private ILogFileService _logFileService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService,
            IUserTokenService userTokenService,
            ILogFileService logFileService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _userTokenService = userTokenService;
            _logFileService = logFileService;
        }

        #region Index
        public async Task<IActionResult> Index(UserViewModel viewModel, int page = 1)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            var model = new UserViewModel
            {
                User = await _userService.GetAllAsync(viewModel.SearchParams, page),
                SearchParams = viewModel.SearchParams
            };

            if (isAjax)
            {
                return PartialView("_PagedPartial", model.User);
            }

            var userRoleId = ViewBag.Authority = _userTokenService.GetCurrentUserRoleId().Result;

            return View(model);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new UserDto()
            {
                Role = await _userService.GetRolesAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = dto.UserName, Name = dto.Name, IsDelete = false, IsLock = false };
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    var role = (RolesEnum)Enum.Parse(typeof(RolesEnum), Convert.ToString(dto.RoleId));

                    await _userManager.AddToRoleAsync(user, role.ToString());//Role(ViewModel.RoleId));

                    await _logFileService.LogInformation("新增使用者", await _userTokenService.GetCurrentUserId());
                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction(nameof(Index));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(dto);

        }
        #endregion

        #region 停用.啟用
        public async Task<IActionResult> IsLock(string id)
        {
            try
            {
                await _userService.LockAsync(id);
            }
            catch (DbUpdateException)
            { }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region 刪除
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.DeleteAsync(id);

                await _logFileService.LogInformation("刪除使用者", await _userTokenService.GetCurrentUserId());
            }
            catch (DbUpdateException)
            { }

            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}