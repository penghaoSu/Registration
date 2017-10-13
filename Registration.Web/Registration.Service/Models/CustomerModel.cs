using Registration.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Service.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public int Industry { get; set; }
        public int Area { get; set; }
        public int Attribute { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }

        public IEnumerable<OrderDto> Order { get; set; }
    }
}
