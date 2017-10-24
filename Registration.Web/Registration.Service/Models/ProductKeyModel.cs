using Registration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class ProductKeyModel : AuthorizeModel
    {
        public int Id { get; set; }

        public ProductKey ProductKey { get; set; }
    }
}
