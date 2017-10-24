using System;
using System.Collections.Generic;

namespace Registration.Data
{
    public partial class ProductKey
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public string LicenceKey { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }

        public OrderDetail OrderDetail { get; set; }
    }
}
