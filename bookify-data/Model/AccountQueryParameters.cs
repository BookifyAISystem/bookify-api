using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
	public class AccountQueryParameters
	{
		// Tìm kiếm
		public string? Search { get; set; }

		// Lọc theo Status (nếu null, không lọc)
		public int? Status { get; set; }

		// Lọc theo RoleId (nếu null, không lọc)
		public int? RoleId { get; set; }

		// Phân trang
		public int Page { get; set; } = 1;       // Trang hiện tại (mặc định = 1)
		public int PageSize { get; set; } = 10;  // Số bản ghi/trang (mặc định = 10)
	}

}
