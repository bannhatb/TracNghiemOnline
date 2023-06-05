using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;
        public FileController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        [HttpPost]
        [Route("upload-file")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadWord([FromForm]IFormFile upload)
        {
            var httpRequest = _httpContext.HttpContext.Request;
            if (httpRequest.Form.Files[0] != null)
            {

                var file = httpRequest.Form.Files[0];
                string filePath = file.FileName;
                string folderName = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
                string fullPath = Path.Combine(folderName, filePath);
                string dbPath = Path.Combine("Upload", filePath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Ok(new Response<ResponseDefault>
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault
                    {
                        Data = fullPath
                    }
                });
            }
            return BadRequest(ErrorCode.NullFileUpload);
        }
        //[HttpGet]
        //[Route("hehe1")]
        //public async Task<IActionResult> test(string id)
        //{
        //    return Ok(id+"abc");
        //}
    }
}
