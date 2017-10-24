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

        [Description("業務人員")]
        Salesperson = 2,

        [Description("交機人員")]
        Deliveryperson = 3,

    }
}
