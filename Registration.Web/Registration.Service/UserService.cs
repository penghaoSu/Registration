using Registration.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registration.Data;
using System.Threading.Tasks;
using Registration.Data.Enums;
using Registration.Data.Enums.Extension;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Registration.Service.Models;
using System.Linq;
using Sakura.AspNetCore;

namespace Registration.Service
{
    public class UserService : ServiceBase, IUserService
    {
        private int pageSize = 5;

        public UserService(IMapper mapper, RegistrationContext context) : base(mapper, context)
        { }

        #region 刪除
        public async Task DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);

            user.IsDelete = true;

            _context.Update(user);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region 取得使用者列表
        public async Task<IEnumerable<UserModel>> GetAllAsync(UserSearch param, int page)
        {
            int pageNumber = page < 1 ? 1 : page;

            var query = _context.AspNetUsers.AsQueryable();

            if (param != null && param.Keyword != null)
            {
                if (param.Selector == "1")
                {
                    query = query.Where(p => p.UserName.Contains(param.Keyword));
                }
                else
                {
                    query = query.Where(p => p.Name.Contains(param.Keyword));
                }
            }

            var data = await query.Select(u => new UserModel
            {
                Id = u.Id,
                Name = u.Name,
                UserName = u.UserName,
                RoleId = u.AspNetUserRoles.Select(r => new AspNetUserRoles
                {
                    RoleId = r.RoleId
                }),
                IsLock = u.IsLock,
                IsDelete = u.IsDelete

            }).Where(u => !u.IsDelete).OrderBy(u => u.Id).AsNoTracking().ToPagedListAsync(pageSize, pageNumber);

            return data;
        }        
        #endregion

        #region 取的單筆使用者
        public async Task<AspNetUsers> GetByIdAsync(string id)
        {
            var result = await _context.AspNetUsers.SingleOrDefaultAsync(u => u.Id == id);

            return result;
        }
        #endregion

        #region 取得角色權限
        public async Task<IEnumerable<SelectListItem>> GetRolesAsync()
        {
            var items = new List<SelectListItem>();

            List<AspNetRoles> roles;

            roles = await _context.AspNetRoles.AsNoTracking().ToListAsync();

            foreach (var role in roles)
            {
                //var enumName = (RolesEnum)Enum.Parse(typeof(RolesEnum), role.Name);

                var item = new SelectListItem()
                {
                    Text = role.Name,//enumName.Description(),//role.Name,
                    Value = role.Id
                };
                items.Add(item);
            }

            return items;
        }
        #endregion

        #region 停用/啟用
        public async Task LockAsync(string id)
        {
            var user = await GetByIdAsync(id);

            user.IsLock = !user.IsLock;

            _context.Update(user);

            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
