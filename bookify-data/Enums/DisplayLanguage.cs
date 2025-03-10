using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Enums
{
    /// <summary>
    /// Ngôn ngữ hiển thị trên giao diện thanh toán VNPAY.  
    /// </summary>
    public enum DisplayLanguage
    {
        /// <summary>
        /// Giao diện hiển thị bằng Tiếng Việt.  
        /// </summary>
        [Description("vn")]
        Vietnamese,

        /// <summary>
        /// Giao diện hiển thị bằng Tiếng Anh.  
        /// </summary>
        [Description("en")]
        English
    }
}
