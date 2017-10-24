using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registration.Service.Models;
using Registration.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Registration.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Key")]
    public class KeyController : Controller
    {
        private IKeyService _keyService;

        public KeyController(IKeyService keyService)
        {
            _keyService = keyService;
        }

        [HttpPost]
        public async Task<Response> GetAuthorize([FromBody]AuthorizeModel model)
        {
            var result = new Response();

            try
            {
                result = await _keyService.GetAuthorize(model);
            }
            catch(DbUpdateException)
            {

            }

            return result;
        }

        [HttpPut]
        public async Task<Response> CancelAuthorize([FromBody]AuthorizeModel model)
        {
            var result = new Response();

            try
            {
                result = await _keyService.CancelAuthorize(model);
            }
            catch (DbUpdateException)
            {

            }

            return result;
        }
    }
}