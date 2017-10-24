using Registration.Data;
using Sakura.AspNetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class OrderViewModel
    {
        public IEnumerable<OrderDto> Order { get; set; }     

        public CustomerSearch SearchParams { get; set; }
    }

    public class CustomerSearch
    {
        public string Selector { get; set; }

        public string Keyword { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
