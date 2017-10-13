using System;
using System.Collections.Generic;

namespace Registration.Data
{
    public partial class CityArea
    {
        public CityArea()
        {
            Customer = new HashSet<Customer>();
        }

        public int Id { get; set; }
        public int CityId { get; set; }
        public string PostalCode { get; set; }
        public string Name { get; set; }

        public City City { get; set; }
        public ICollection<Customer> Customer { get; set; }
    }
}
