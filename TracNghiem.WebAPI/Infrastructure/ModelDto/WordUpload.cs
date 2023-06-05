using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.ModelDto
{
    public class WordUpload
    {
        public string FileUp { get; set; }
        public string SplitNumberAndContent { get; set; }
        public string rightMark { get; set; }
        public int examId { get; set; }

    }
}
