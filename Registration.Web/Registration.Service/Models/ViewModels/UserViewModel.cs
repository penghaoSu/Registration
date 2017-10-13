using Registration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class UserViewModel
    {
        public IEnumerable<AspNetUsers> User { get; set; }

        public UserSearch SearchParams { get; set; }
    }

    public class UserSearch
    {
        public string Selector { get; set; }

        public string Keyword { get; set; }
    }
}
