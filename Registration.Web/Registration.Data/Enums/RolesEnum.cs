using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Registration.Data.Enums
{
    public enum RolesEnum
    {
        [Description("系統管理員")]
        Administrator = 1,

        [Description("一般人員")]
        GeneralStaff = 2,

    }
}
