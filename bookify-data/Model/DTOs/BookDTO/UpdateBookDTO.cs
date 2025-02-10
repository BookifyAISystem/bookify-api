﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_api.DTOs.BookDTO
{
    public class UpdateBookDTO
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? BookImage { get; set; }
        public string? BookType { get; set; }
        public int? Price { get; set; }
        public int? PriceEbook { get; set; }
        public string? Description { get; set; }
        public string? BookContent { get; set; }
        public int PulishYear { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int CollectionId { get; set; }
        public int PromotionId { get; set; }
    }
}
