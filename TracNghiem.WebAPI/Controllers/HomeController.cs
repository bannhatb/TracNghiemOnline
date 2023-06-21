using MediatR;
using Microsoft.AspNetCore.Mvc;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using TracNghiem.WebAPI.Services;

namespace TracNghiem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILevelRepository _levelRepository;

        public HomeController(
            IMediator mediator,
            IHttpContextAccessor httpContext,
            ILevelRepository levelRepository
            )
        {
            _mediator = mediator;
            _httpContext = httpContext;
            _levelRepository = levelRepository;
        }


        [HttpGet]
        [Route("get-username")]
        public IActionResult GetUserNameCurrent()
        {
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = _httpContext.HttpContext.User.Identity.Name.ToString()
                }
            });
        }

        [HttpGet]
        [Route("get-all-level")]
        [ProducesResponseType(typeof(Response<Pagination<Level>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllLevel()
        {
            List<Level> levels = await _levelRepository.GetLevels();
            if (levels == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB
                });
            return Ok(new Response<Pagination<Level>>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new Pagination<Level>()
                {
                    Items = levels,
                    Total = levels.Count()
                }
            });
        }
    }
}