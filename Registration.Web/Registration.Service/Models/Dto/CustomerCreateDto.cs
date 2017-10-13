using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class CustomerCreateDto
    {
        public string Industry { get; set; }
        public string Area { get; set; }
        public string Attribute { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string Salesperson { get; set; }
        public string Deliveryperson { get; set; }
        public bool IsReceipt { get; set; }
        public int? AuthorizeType { get; set; }
        public string Version { get; set; }
        public string Revision { get; set; }
        public string Module { get; set; }
        public string Warranty { get; set; }
        public DateTime? WarrantyPeriodStr { get; set; }
        public DateTime? WarrantyPeriodEnd { get; set; }
        public string Lease { get; set; }
        public DateTime? LeaseDateEnd { get; set; }
        public DateTime? LeaseDateStr { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public bool IsAutoUpdate { get; set; }
        public int? Status { get; set; }
    }
}
