﻿using System;
using System.Collections.Generic;

namespace Registration.Data
{
    public partial class Order
    {
        public Order()
        {
            ProductKey = new HashSet<ProductKey>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrderType { get; set; }
        public int Module { get; set; }
        public int Number { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string Salesperson { get; set; }
        public string Deliveryperson { get; set; }
        public bool IsReceipt { get; set; }
        public int? AuthorizeType { get; set; }
        public string Version { get; set; }
        public string Revision { get; set; }
        public int? Warranty { get; set; }
        public DateTime? WarrantyPeriodStr { get; set; }
        public DateTime? WarrantyPeriodEnd { get; set; }
        public int? Lease { get; set; }
        public DateTime? LeaseDateEnd { get; set; }
        public DateTime? LeaseDateStr { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public bool IsAutoUpdate { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }

        public Customer Customer { get; set; }
        public ICollection<ProductKey> ProductKey { get; set; }
    }
}
