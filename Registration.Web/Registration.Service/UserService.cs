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

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AspNetUsers>> GetAllAsync(UserSearch param, int page)
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

            var data = await query.OrderBy(c => c.Id).AsNoTracking().ToPagedListAsync(pageSize, pageNumber);

            return data;
        }

        public Task<AspNetUsers> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

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
    }
}
