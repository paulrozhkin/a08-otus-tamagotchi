using System;
using System.Collections.Generic;

namespace Web.HttpAggregator.Models
{
    public class PaginationResponse<T>
    {
        public long CurrentPage { get; set; }

        public long PageSize { get; set; }

        public long TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public int TotalCount { get; set; }

        public List<T> Items { get; set; }
    }
}
