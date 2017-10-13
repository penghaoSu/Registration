using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public int Industry { get; set; }
        public int Area { get; set; }
        public int Attribute { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public DateTime CreateDate { get; set; }
        public int CityId { get; set; }
        public int CityAreaId { get; set; }
        public string Address { get; set; }
    }
}
