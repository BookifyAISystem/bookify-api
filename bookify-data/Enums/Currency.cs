using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Enums
{
    /// <summary>
    /// Đơn vị tiền tệ sử dụng cho giao dịch
    /// </summary>
    public enum Currency
    {
        [Description("Việt Nam đồng")]
        VND,

        //[Description("Đô la Mỹ")]
        //USD,
    }
}
