using Registration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models.ViewModels
{
    public class CustomerOrderViewModel
    {
        public int Id { get; set; }
        public int Industry { get; set; }
        public int Area { get; set; }
        public int Attribute { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }

        public IEnumerable<Order> Order { get; set; }
    }
}
