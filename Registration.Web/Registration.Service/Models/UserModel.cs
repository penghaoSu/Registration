using Registration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public IEnumerable<AspNetUserRoles> RoleId { get; set; }

        public bool IsLock { get; set; }

        public bool IsDelete { get; set; }
    }
}
