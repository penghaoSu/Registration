using Registration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public string Deliveryperson { get; set; }
        public int AuthorizeType { get; set; }
        public string Version { get; set; }
        public string Revision { get; set; }
        public int Warranty { get; set; }
        public DateTime WarrantyPeriodStr { get; set; }
        public DateTime WarrantyPeriodEnd { get; set; }
        public int Lease { get; set; }
        public DateTime LeaseDateEnd { get; set; }
        public DateTime LeaseDateStr { get; set; }
        public string SerialNumber { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsAutoUpdate { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string Remark { get; set; }

        //public Order Order { get; set; }
        //public IEnumerable<ProductKey> ProductKey { get; set; }
    }
}
