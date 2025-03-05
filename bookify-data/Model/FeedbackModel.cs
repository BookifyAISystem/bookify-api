using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class GetFeedbackDTO
    {
        public int FeedbackId { get; set; }
        public int Star { get; set; }
        public string? FeedbackContent { get; set; }
        public int AccountId { get; set; }
        public int BookId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }


    }



    public class AddFeedbackDTO
    {
        public int Star { get; set; }
        public string? FeedbackContent { get; set; }
        public int AccountId { get; set; }
        public int BookId { get; set; }
    }
    public class UpdateFeedbackDTO
    {
        public int Star { get; set; }
        public string? FeedbackContent { get; set; }
        public int AccountId { get; set; }
        public int Status { get; set; }

    }
}
