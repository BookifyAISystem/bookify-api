using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class GetCategoryDTO
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
    }

    public class AddCategoryDTO
    {
        public string? CategoryName { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public string? CategoryName { get; set; }
        public int Status { get; set; }
    }
}
