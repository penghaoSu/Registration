using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Registration.Data.Enums
{
    public enum IndustryEnum
    {
        [Description("餐影業")]
        FreeTrial = 1,

        [Description("資訊業")]
        NotStart = 2,

        [Description("營造業")]
        Normal = 3,
    }
}
