using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Registration.Data.Enums;
using Registration.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Web.Services
{
    public class UserTokenService : IUserTokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserTokenService(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(_contextAccessor.HttpContext.User);

        public async Task<string> GetCurrentUserId()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            return usr?.Id;
        }

        public async Task<string> GetCurrentUserName()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            return usr?.Name;
        }

        public async Task<string> GetCurrentUserRoleId()
        {
            ApplicationUser usr = await GetCurrentUserAsync();

            var roles = await _userManager.GetRolesAsync(usr);

            return roles[0];
        }

        public async Task<bool> GetUserDelete()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            return usr.IsDelete;
        }

        public async Task<bool> GetUserLock()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            return usr.IsLock;
        }
    }
}
