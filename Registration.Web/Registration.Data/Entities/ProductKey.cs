using System;
using System.Collections.Generic;

namespace Registration.Data
{
    public partial class ProductKey
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Key { get; set; }
        public DateTime Date { get; set; }

        public Order Order { get; set; }
    }
}
