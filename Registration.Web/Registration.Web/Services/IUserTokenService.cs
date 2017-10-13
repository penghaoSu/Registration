using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Web.Services
{
    public interface IUserTokenService
    {
        /// <summary>
        /// 取得登入者ID
        /// </summary>
        /// <returns></returns>
        Task<string> GetCurrentUserId();

        /// <summary>
        /// 取得登入者名稱
        /// </summary>
        /// <returns></returns>
        Task<string> GetCurrentUserName();

        /// <summary>
        /// 取得登入者角色權限ID
        /// </summary>
        /// <returns></returns>
        Task<string> GetCurrentUserRoleId();

        /// <summary>
        /// 取得登入者門市ID
        /// </summary>
        /// <returns></returns>
        //Task<int?> GetCurrentUserStoreId();
    }
}
