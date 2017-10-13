using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Registration.Data.Enums
{
    public enum AreaEnum
    {
        [Description("北區")]
        FreeTrial = 1,

        [Description("中區")]
        NotStart = 2,

        [Description("南區")]
        Normal = 3,
    }
}
