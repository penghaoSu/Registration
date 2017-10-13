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
        // 取列表
        Task<IEnumerable<AspNetUsers>> GetAllAsync(UserSearch param, int page);

        /// <summary>
        /// 取所有角色權限
        /// </summary>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        Task<IEnumerable<SelectListItem>> GetRolesAsync();

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// 取得單筆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AspNetUsers> GetByIdAsync(string id);

    }
}
