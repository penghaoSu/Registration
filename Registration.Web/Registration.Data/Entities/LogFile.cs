using System;
using System.Collections.Generic;

namespace Registration.Data
{
    public partial class LogFile
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Item { get; set; }
        public string UserId { get; set; }

        public AspNetUsers User { get; set; }
    }
}
