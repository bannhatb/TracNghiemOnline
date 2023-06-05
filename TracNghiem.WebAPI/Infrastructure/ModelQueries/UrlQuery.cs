using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.ModelQueries
{
    public class UrlQuery
    {
        public string Keyword { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public UrlQuery()
        {
            Keyword = "";
            Page = 1;
            PageSize = 10;
        }
    }
}
