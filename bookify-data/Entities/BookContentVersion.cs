using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bookify_data.Entities
{
    public class BookContentVersion
    {
        public int BookContentVersionId { get; set; }
        public int BookId { get; set; }

      
        public string? Summary1 { get; set; }
        public string? Summary2 { get; set; }
        public string? Summary3 { get; set; }
        public string? Summary4 { get; set; }
        public string? Summary5 { get; set; }

        public int Version { get; set; } = 1;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastEdited { get; set; } = DateTime.UtcNow;
        public int Status { get; set; } = 1;

        // Navigation property
        public Book? Book { get; set; }
    }
}
