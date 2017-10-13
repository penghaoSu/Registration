using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Registration.Data.Enums.Extension
{
    public enum IndustryEnum
    {
        [Description("試用")]
        FreeTrial = 1,

        [Description("未啟動")]
        NotStart = 2,

        [Description("正常")]
        Normal = 3,

        [Description("過保")]
        Overbought = 4,

        [Description("異常")]
        Abnormal = 5,
    }
}
