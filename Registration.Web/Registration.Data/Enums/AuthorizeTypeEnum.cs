using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Registration.Data.Enums
{
    public enum AuthorizeTypeEnum
    {
        [Description("租用")]
        Rented = 1,

        [Description("買斷")]
        RoyaltyFree = 2,

        [Description("試用")]
        FreeTrial = 3,

    }
}
