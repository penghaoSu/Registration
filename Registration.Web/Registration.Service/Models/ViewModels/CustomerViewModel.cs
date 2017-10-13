using Registration.Data;
using Sakura.AspNetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class CustomerViewModel
    {
        public IEnumerable<CustomerModel> Customer { get; set; }     

        public CustomerSearch SearchParams { get; set; }
    }

    public class CustomerSearch
    {
        public string Keyword { get; set; }

        public string Name { get; set; }
    }
}
