using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
    }

}
