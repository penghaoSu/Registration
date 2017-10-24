using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Registration.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Registration.Service.Models;
using Registration.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Registration.Web.Services;
using Registration.Data;

namespace Registration.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ICustomerService _customerService;
        private IUserTokenService _userTokenService;
        private ILogFileService _logFileService;

        public HomeController(ICustomerService customerService, IUserTokenService userTokenService , ILogFileService logFileService)
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

            if (viewModel.SearchParams != null)
            {
                if (viewModel.SearchParams.Keyword != null)
                    TempData["keyword"] = true;
            }

            if (isAjax)
            {
                return PartialView("_PagedPartial", model.Order);
            }

            var userRoleId = ViewBag.Authority = _userTokenService.GetCurrentUserRoleId().Result;

            return View(model);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create(int? id)
        {
            var model = new CustomerCreateViewModel()
            {
                City = await _customerService.GetCityAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel model , IDictionary<Guid, OrderDetailDto> details)//, IDictionary<Guid, SerialNumber> sn)
        {
            try
            {
                model.OrderDetailDto = details.Values;

                await _customerService.CreateOrderAsync(model);

                await _logFileService.LogInformation("新增客戶", await _userTokenService.GetCurrentUserId());

                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException ex)
            {

            }

            model.City = await _customerService.GetCityAsync();

            return View(model);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            var model = await _customerService.GetOrderByCustomerIdAsync(id.Value);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerOrderViewModel viewModel)
        {
            try
            {
                //await _customerService.EditAsync(id, viewModel.Order);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "更新失敗，請再試一次，若還是失敗，請聯絡您的系統管理員");
            }

            return View(viewModel);
        }

        #endregion

        #region Display
        public async Task<IActionResult> Display(int id)
        {
            try
            {
                var model = await _customerService.GetOrderByCustomerIdAsync(id);

                return View(model);
            }
            catch (DbUpdateException)
            {

            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _customerService.DeleteAsync(id);

                if (!result)
                {
                    ModelState.AddModelError("", "無此筆資料");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "刪除失敗");
            }


            return RedirectToAction("Index");
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

        public async Task<IActionResult> ResetProductKey(int id)
        {
            try
            {
                await _customerService.ResetProductKey(id);
            }
            catch (DbUpdateException ex)
            {
                throw (ex);
            }

            var model = await _customerService.GetProductKeyByOrderId(id);

            return PartialView("_ProductKey", model);
        }

        public IActionResult AddSn(int Id)
        {
            return PartialView("_SerialNumber");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> AllUser()
        {
            var model = await _customerService.GetAllUserAsync();

            return PartialView("_AllUser", model);
        }

        public IActionResult AddDetail()
        {
            return PartialView("_OrderDetail", new OrderDetailDto { });
        }
    }
}
