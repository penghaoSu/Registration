using System;
using System.Collections.Generic;

namespace Registration.Data
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int Industry { get; set; }
        public int Area { get; set; }
        public int Attribute { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public int? CityId { get; set; }
        public int? CityAreaId { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }

        public City City { get; set; }
        public CityArea CityArea { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
