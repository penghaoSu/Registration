using System;
using System.Collections.Generic;

namespace Registration.Data
{
    public partial class Order
    {
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrderType { get; set; }
        public int Module { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string Salesperson { get; set; }
        public bool IsReceipt { get; set; }

        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
