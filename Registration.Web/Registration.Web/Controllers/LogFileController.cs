using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Registration.Service.Interface;

namespace Registration.Web.Controllers
{
    public class LogFileController : Controller
    {
        private ILogFileService _logFileService;

        public LogFileController(ILogFileService logFileService)
        {
            _logFileService = logFileService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            var model = await _logFileService.GetAllLogAsync(page);

            if (isAjax)
            {
                return PartialView("_PagedPartial", model);
            }

            return View(model);
        }
    }
}