using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }
        public string? Username { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int RoleId { get; set; }

        // Nếu muốn lấy tên role, bạn có thể thêm:
        public string? RoleName { get; set; }

        // Lấy danh sách Note (nếu cần chi tiết)
        public List<NoteViewModel> Notes { get; set; } = new List<NoteViewModel>();

    }
}
