using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class AuthorizeModel
    {
        public string Version { get; set; }

        public string SerialNumber { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}
