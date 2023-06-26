using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Application.Commands.AdminCommands;
using TracNghiem.WebAPI.Infrastructure.ModelQueries;
using TracNghiem.WebAPI.Infrastructure.Queries;
using TracNghiem.WebAPI.Infrastructure.Response;
using TracNghiem.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracNghiem.Domain.Entities;

namespace TracNghiem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppQueries _appQueries;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IExamRepository _examRepository;
        private readonly ITestRepository _testRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        public AdminController(IMediator mediator,
            IExamRepository examRepository,
            IQuestionRepository questionRepository,
            IHttpContextAccessor httpContext,
            IAppQueries appQueries,
            IUserRepository userRepository,
            ITestRepository testRepository)
        {
            _mediator = mediator;
            _appQueries = appQueries;
            _examRepository = examRepository;
            _userRepository = userRepository;
            _httpContext = httpContext;
            _questionRepository = questionRepository;
            _testRepository = testRepository;
        }
        [HttpGet]
        [Route("get-all-user")]
        [CustomAuthorize(Allows = "Admin")]
        [ProducesResponseType(typeof(Response<Pagination<UserQueryModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUser([FromQuery] UrlQuery urlQuery, [FromQuery] List<int> classIds)
        {
            List<UserQueryModel> listUser = await _appQueries.GetAllUser(urlQuery, classIds);
            int count = await _appQueries.CountGetllUser(urlQuery, classIds);
            foreach (UserQueryModel user in listUser)
            {
                user.ListRoles = await _appQueries.GetRoleUser(user.UserId);
            }
            return Ok(new Response<Pagination<UserQueryModel>>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new Pagination<UserQueryModel>()
                {
                    Total = count,
                    Items = listUser
                }
            });
        }
        [HttpGet]
        [Route("get-all-exam")]
        [ProducesResponseType(typeof(Response<Pagination<ExamQueryModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllExam([FromQuery] UrlQuery urlQuery)
        {
            List<ExamQueryModel> exams = await _appQueries.GetAllExam(urlQuery);
            int countExam = await _appQueries.CountExam(urlQuery);
            if (exams == null)
                return BadRequest(new Response<Pagination<ExamQueryModel>>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                });
            return Ok(new Response<Pagination<ExamQueryModel>>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new Pagination<ExamQueryModel>()
                {
                    Total = countExam,
                    Items = exams
                }
            });
        }
        [HttpGet]
        [Route("get-all-test")]
        [CustomAuthorize(Allows = "Admin")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTest([FromQuery] UrlQuery urlQuery)
        {
            List<TestUserQueryModel> tests = await _appQueries.GetListTestInfo(urlQuery);
            int total = await _appQueries.CountGetListTestInfo(urlQuery);
            if (tests.Count() == 0)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                });

            return Ok(new Response<Pagination<TestUserQueryModel>>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new Pagination<TestUserQueryModel>()
                {
                    Items = tests,
                    Total = total
                }
            });
        }
        [HttpPost]
        [Route("block-user")]
        [CustomAuthorize(Allows = "Admin")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddExam([FromBody] BlockUserCommand command)
        {
            if (command == null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);

            if (result.State)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet]
        [Route("get-list-test-brief")]
        [CustomAuthorize(Allows = "Admin")]
        [ProducesResponseType(typeof(Response<Pagination<TestUserQueryModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListTestBrief([FromQuery] UrlQuery urlQuery)
        {
            List<TestUserQueryModel> tests = await _appQueries.GetListTestInfo(urlQuery);
            int total = await _appQueries.CountGetListTestInfo(urlQuery);
            if (tests.Count() == 0)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                });

            return Ok(new Response<Pagination<TestUserQueryModel>>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new Pagination<TestUserQueryModel>()
                {
                    Items = tests,
                    Total = total
                }
            });
        }
        [HttpGet]
        [Route("get-user-detail")]
        [ProducesResponseType(typeof(Response<Pagination<UserQueryModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserDetail([FromQuery] int userId, [FromQuery] UrlQuery urlQuery)
        {
            string userName = _userRepository.Users.FirstOrDefault(x => x.Id == userId).UserName;
            UserDetailQueryModel userDetail = await _appQueries.GetUserDetail(userId);
            userDetail.ListExams = await _appQueries.GetListExam(userName, urlQuery);
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
        [HttpPost]
        [Route("authen-teacher")]
        [CustomAuthorize(Allows = "Admin")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AuthenTeacher([FromBody] AuthenTeacherCommand command)
        {
            if (command == null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);

            if (result.State)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost]
        [Route("authen-admin")]
        [CustomAuthorize(Allows = "Admin")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AuthenAdmin([FromBody] AuthenAdminCommand command)
        {
            if (command == null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);

            if (result.State)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost]
        [Route("delete-exam-admin")]
        [CustomAuthorize(Allows = "Admin")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteExam([FromBody] int id)
        {
            if (id.ToString() is null)
            {
                return BadRequest(null);
            }

            var exam = await _examRepository.Exams.FirstOrDefaultAsync(x => x.Id == id);
            if (exam is null)
            {
                return NotFound(ErrorCode.NotFound);
            }
            _examRepository.Delete(exam);

            var result = await _examRepository.UnitOfWork.SaveAsync();

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Xóa thành công exam"
                    }
                });
            }

            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Lỗi khi xóa exam"
                }
            });
        }

        [HttpGet]
        [Route("get-all-question")]
        [CustomAuthorize(Allows = "Admin")]
        [ProducesResponseType(typeof(Response<Pagination<Question>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllQuestion()
        {
            var listQuestion = _questionRepository.GetListAllQuestion();
            if (listQuestion == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB
                });

            return Ok(new Response<Pagination<Question>>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new Pagination<Question>()
                {
                    Items = listQuestion,
                    Total = listQuestion.Count()
                }
            });
        }
    }
}
