using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Registration.Data.Enums
{
    public enum ModuleEnum
    {
        [Description("庫存")]
        Stock = 1,

        [Description("配方")]
        Formula = 2,

        [Description("促銷")]
        Promotion = 3,

        [Description("APP")]
        APP = 4,

    }
}
