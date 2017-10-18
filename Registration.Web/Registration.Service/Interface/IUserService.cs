using Microsoft.AspNetCore.Mvc.Rendering;
using Registration.Data;
using Registration.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Service.Interface
{
    public interface IUserService
    {
        /// <summary>
        /// 取得使用者列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<IEnumerable<UserModel>> GetAllAsync(UserSearch param, int page);
                
        /// <summary>
        /// 取得角色權限
        /// </summary>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        Task<IEnumerable<SelectListItem>> GetRolesAsync();

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(string id);

        /// <summary>
        /// 停用/啟用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task LockAsync(string id);

        /// <summary>
        /// 取得單筆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AspNetUsers> GetByIdAsync(string id);

    }
}
