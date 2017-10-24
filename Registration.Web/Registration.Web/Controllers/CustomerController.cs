using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Registration.Service.Interface;
using Registration.Web.Services;
using Registration.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Registration.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private ICustomerService _customerService;
        private IUserTokenService _userTokenService;
        private ILogFileService _logFileService;

        public CustomerController(ICustomerService customerService, IUserTokenService userTokenService, ILogFileService logFileService)
        {
            _customerService = customerService;

            _userTokenService = userTokenService;

            _logFileService = logFileService;
        }

        #region Index
        public async Task<IActionResult> Index(OrderViewModel viewModel, int page = 1)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            var model = new OrderViewModel
            {
                Order = await _customerService.GetAllCustomerAsync(viewModel.SearchParams, page),
                SearchParams = viewModel.SearchParams
            };

            if (isAjax)
            {
                return PartialView("_PagedPartial", model.Order);
            }

            var userRoleId = ViewBag.Authority = _userTokenService.GetCurrentUserRoleId().Result;

            return View(model);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            var model = new CustomerCreateViewModel()
            {
                City = await _customerService.GetCityAsync()
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel model, IDictionary<Guid, SerialNumber> sn)
        {
            try
            {
                //model.SerialNumber = sn.Values;

                await _customerService.CreateOrderAsync(model);

                await _logFileService.LogInformation("新增客戶", await _userTokenService.GetCurrentUserId());

                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException)
            {

            }

            model.City = await _customerService.GetCityAsync();

            return View(model);
        }
        #endregion

        #region Create Module
        public async Task<IActionResult> CreateModule(int? id)
        {
            try
            {
                var model = new CustomerCreateViewModel()
                {
                    CustomerNew = await _customerService.GetCustomerByIdAsync(id.Value),
                    City = await _customerService.GetCityAsync(),
                    OrderDto = new OrderDto()
                };

                return View(model);
            }
            catch (DbUpdateException)
            {

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModule(CustomerCreateViewModel model, IDictionary<Guid, SerialNumber> sn)
        {
            try
            {
                //model.SerialNumber = sn.Values;

                await _customerService.CreateModuleAsync(model);

                await _logFileService.LogInformation("新增模組", await _userTokenService.GetCurrentUserId());

                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException)
            {

            }

            return View();
        }
        #endregion

        #region 地區
        [HttpPost]
        public async Task<JsonResult> CityArea(int cityId)
        {
            var model = await _customerService.GetCityAreaAsync(cityId);

            return Json(model);
        }
        #endregion

        #region 客戶所有訂單資料
        public async Task<IActionResult> AllOrder(int Id)
        {
            try
            {
                var model = await _customerService.GetAllOrderByAsync(Id);

                return PartialView("_AllOrder", model);
            }
            catch (DbUpdateException)
            {

            }


            return PartialView("_AllOrder");
        }
        #endregion

        public IActionResult AddSn(int Id)
        {
            var model = new SerialNumber()
            {
                Flag = false
            };

            return PartialView("_SerialNumber", model);
        }

        public async Task<IActionResult> AllUser()
        {
            var model = await _customerService.GetAllUserAsync();

            return PartialView("_AllUser", model);
        }

    }
}