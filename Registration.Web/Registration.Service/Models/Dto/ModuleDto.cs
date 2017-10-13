using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Models
{
    public class ModuleDto : OrderDto
    {
        public int Cid { get; set; }

        public int Module { get; set; }

        public IEnumerable<SerialNumber> ModuleSerialNumber { get; set; }
    }
}
