using MediatR;
using Microsoft.AspNetCore.Mvc;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using TracNghiem.WebAPI.Services;
using TracNghiem.WebAPI.Infrastructure.ModelQueries;
using TracNghiem.WebAPI.Infrastructure.Queries;

namespace TracNghiem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]
    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILevelRepository _levelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppQueries _appQueries;

        public HomeController(
            IMediator mediator,
            IHttpContextAccessor httpContext,
            ILevelRepository levelRepository,
            IUserRepository userRepository,
            IAppQueries appQueries
            )
        {
            _mediator = mediator;
            _httpContext = httpContext;
            _levelRepository = levelRepository;
            _userRepository = userRepository;
            _appQueries = appQueries;
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

        [HttpGet]
        [Route("get-user-detail-current")]
        [ProducesResponseType(typeof(Response<Pagination<UserQueryModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserDetailCurrent()
        {
            string userName = _httpContext.HttpContext.User.Identity.Name.ToString();
            int userId = _userRepository.Users.FirstOrDefault(x => x.UserName == userName).Id;
            UserDetailQueryModel userDetail = await _appQueries.GetUserDetail(userId);
            // userDetail.ListExams = await _appQueries.GetListExam(userName, urlQuery);
            userDetail.ListRoles = await _appQueries.GetRoleUser(userDetail.UserId);
            userDetail.ListTestCreate = await _appQueries.GetListTestCreateByUser(userName);
            userDetail.ListTestDid = await _appQueries.GetListTestDidByUser(userId);
            if (userDetail == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.BadRequest,
                    Result = new ResponseDefault()
                    {
                        Data = "khong ton tai user"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = userDetail
                }
            });
        }
    }
}