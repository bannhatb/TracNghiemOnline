using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.Response
{
    public class Response<T> where T : class
    {
        public bool State { get; set; } // true false?
        public string Message { get; set; } // error Code
        public T Result { get; set; } // data tra ve allow null // may be pagination
    }
    public class ResponseDefault
    {
        public object Data { get; set; }
    }
    public class Pagination<T>
    {
        public int Total { get; set; }
        public List<T> Items { get; set; }  
    }
    public class ValidatorError
    {
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
    public class ResponseToken
    {
        public string Token { get; set; }
    }
}
