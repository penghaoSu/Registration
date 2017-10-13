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

namespace Registration.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IUserTokenService _userTokenService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService,
            IUserTokenService userTokenService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _userTokenService = userTokenService;
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
                var user = new ApplicationUser { UserName = dto.UserName, Name = dto.Name };
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    var role = (RolesEnum)Enum.Parse(typeof(RolesEnum), Convert.ToString(dto.RoleId));

                    await _userManager.AddToRoleAsync(user, role.ToString());//Role(ViewModel.RoleId));

                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction(nameof(Index));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(dto);

        }
        #endregion

    }
}