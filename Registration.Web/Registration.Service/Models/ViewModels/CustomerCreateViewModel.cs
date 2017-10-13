using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class CustomerCreateViewModel
    {
        public CustomerDto CustomerNew { get; set; }

        public OrderDto OrderDto { get; set; }

        public IEnumerable<SerialNumber> SerialNumber { get; set; }

        public IEnumerable<SelectListItem> City { get; set; }

        public IEnumerable<SelectListItem> CityArea { get; set; }
    }

    public class SerialNumber
    {
        public string SN { get; set; }

        public bool? Flag { get; set; }
    }
}
