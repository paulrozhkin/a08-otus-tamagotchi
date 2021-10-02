using System;
using System.Collections.Generic;

namespace Web.HttpAggregator.Models
{
    public class PaginationResponse<T>
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public int TotalCount { get; set; }

        public List<T> Items { get; set; }
    }
}
