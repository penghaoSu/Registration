using Registration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrderType { get; set; }
        public int Module { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Salesperson { get; set; }
        public bool IsReceipt { get; set; }

        public Customer Customer { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
