using System;
using System.Collections.Generic;

namespace Registration.Data
{
    public partial class City
    {
        public City()
        {
            CityArea = new HashSet<CityArea>();
            Customer = new HashSet<Customer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CityArea> CityArea { get; set; }
        public ICollection<Customer> Customer { get; set; }
    }
}
