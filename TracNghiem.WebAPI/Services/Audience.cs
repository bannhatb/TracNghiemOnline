using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Services
{
    public class Audience
    {
        public string Name { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
    }
}
