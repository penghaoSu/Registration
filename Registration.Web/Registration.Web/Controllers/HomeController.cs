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

namespace Registration.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ICustomerService _customerService;
        private IUserTokenService _userTokenService;

        public HomeController(ICustomerService customerService , IUserTokenService userTokenService)
        {
            _customerService = customerService;

            _userTokenService = userTokenService;
        }

        #region Index
        public async Task<IActionResult> Index(CustomerViewModel viewModel, int page = 1)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            var model = new CustomerViewModel
            {
                Customer = await _customerService.GetAllAsync(viewModel.SearchParams, page),
                SearchParams = viewModel.SearchParams
            };

            if (isAjax)
            {
                return PartialView("_PagedPartial", model.Customer);
            }

            var userRoleId = ViewBag.Authority = _userTokenService.GetCurrentUserRoleId().Result;

            return View(model);
        }
        #endregion

        #region Create New
        public IActionResult CreateNew()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(CustomerCreateViewModel model)
        {
            try
            {
                await _customerService.CreateAsync(model);

                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException ex)
            {

            }

            return View();
        }
        #endregion

        #region Create Old
        public IActionResult CreateOld()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOld(CustomerCreateViewModel model)
        {
            try
            {
                await _customerService.CreateAsync(model);

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {

            }

            return View();
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
                    OrderDto = new OrderDto()
                };

                return View(model);
            }
            catch(DbUpdateException)
            {

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModule(CustomerCreateViewModel model)
        {
            try
            {
                await _customerService.CreateModuleAsync(model);

                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {

            }

            return View();
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _customerService.GetCustomerByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            try
            {
                await _customerService.EditAsync(id, dto);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "更新失敗，請再試一次，若還是失敗，請聯絡您的系統管理員");
            }

            return View(dto);
        }

        #endregion

        #region Display
        public async Task<IActionResult> Display(int id)
        {
            try
            {
                var model = await _customerService.GetCustomerByIdAsync(id);

                return View(model);
            }
            catch (DbUpdateException ex)
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
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "刪除失敗");
            }


            return RedirectToAction("Index");
        }
        #endregion

        public async Task<IActionResult> AddSn(int Id)
        {
            return PartialView("_SerialNumber");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
