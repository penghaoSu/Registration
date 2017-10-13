using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Registration.Data.Enums
{
    public enum AttributeEnum
    {
        [Description("一般")]
        FreeTrial = 1,

        [Description("倒閉客")]
        NotStart = 2,

        [Description("test")]
        Normal = 3,
    }
}
