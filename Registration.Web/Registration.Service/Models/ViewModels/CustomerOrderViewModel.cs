using Microsoft.AspNetCore.Mvc.Rendering;
using Registration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class CustomerOrderViewModel
    {
        public Customer Customer { get; set; }

        public IEnumerable<Order> Order { get; set; }

        public IEnumerable<OrderDetail> OrderDetail { get; set; }

        public IEnumerable<SelectListItem> City { get; set; }

        public IEnumerable<SelectListItem> CityArea { get; set; }
    }
}
