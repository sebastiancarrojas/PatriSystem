using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatriSystem.Domain.Pagination
{
    public class BrandPaginationRequest : PaginationRequest
    {
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}
