using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Application.Commands.TestCommands;
using TracNghiem.WebAPI.Infrastructure.ModelQueries;
using TracNghiem.WebAPI.Infrastructure.Queries;
using TracNghiem.WebAPI.Infrastructure.Response;
using TracNghiem.WebAPI.Services;
using TracNghiem.WebAPI.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Controllers
{
    [Route("api/Test")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppQueries _appQueries;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITestRepository _testRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<TracnghiemHub> _hubContext;
        public TestController(IMediator mediator,
            IAppQueries appQueries,
            IHttpContextAccessor httpContext,
            IQuestionRepository questionRepository,
            ITestRepository testRepository,
            IHubContext<TracnghiemHub> hubContext,
            IUserRepository userRepository)
        {
            _mediator = mediator;
            _appQueries = appQueries;
            _httpContext = httpContext;
            _questionRepository = questionRepository;
            _testRepository = testRepository;
            _hubContext = hubContext;
            _userRepository = userRepository;
        }
        [HttpPost]
        [Route("create-test")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateTest(AddTestCommand command)
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
        [Route("create-test-user")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateTestUser(CreateTestUserCommand command)
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
        [Route("get-user-test-status")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserTestStatus([FromQuery] int testId)
        {
            int userId = Convert.ToInt32(_httpContext.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            TestUserStatusQueryModel userTest = await _appQueries.GetUserTestStatus(testId, userId);

            if (userTest == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                });

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = userTest
                }
            });
        }
        [HttpGet]
        [Route("get-user-test")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserTest([FromQuery] int testId, [FromQuery] UrlQuery urlQuery)
        {
            TestUserQueryModel test = await _appQueries.GetTestInfo(testId);

            if (test == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                });
            int userId = Convert.ToInt32(_httpContext.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            List<TestQuestionQueryModel> questions = await _appQueries.GetQuestionTest(userId, testId, urlQuery);
            foreach (TestQuestionQueryModel question in questions)
            {
                List<AnswerQueryModel> answers = await _appQueries.GetAnswerQuestion(question.QuestionId);
                List<TestQuestionResult> userChoose = await _appQueries.GetUserAnswer(question.Id);
                question.UserChoose = userChoose;
                question.ListAnswers = answers;
                //question.ListCategories = categories;
            }
            test.questions = questions;

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = test
                }
            });
        }
        [HttpGet]
        [Route("get-one-question-user-test")]
        [ProducesResponseType(typeof(Response<TestQuestionQueryModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOneQuestionUserTest([FromQuery] int testId, [FromQuery] int page)
        {
            int userId = Convert.ToInt32(_httpContext.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            TestQuestionQueryModel question = await _appQueries.GetOneQuestionTest(userId, testId, page);
            int total = await _appQueries.CountGetOneQuestionTest(userId, testId);
            List<AnswerQueryModel> answers = await _appQueries.GetAnswerQuestion(question.QuestionId);
            List<TestQuestionResult> userChoose = await _appQueries.GetUserAnswer(question.Id);
            question.UserChoose = userChoose;
            question.ListAnswers = answers;
            var result = new
            {
                questions = question,
                total = total
            };
            //question.ListCategories = categories;
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = result
                }
            });
        }
        [HttpPost]
        [Route("press-time")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PressTime(StartUserTestCommand command)
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
        [Route("user-choose-answer")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UserChoose([FromBody] ChooseAnswerCommand command)
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
        [Route("caculate-point")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CaculPoint([FromBody] CaculPointCommand command)
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
        [Route("get-test-result")]
        [CustomAuthorize(Allows = "Teacher,Admin")]
        [ProducesResponseType(typeof(Response<Pagination<TestResultQueryModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTestResult([FromQuery] int testId, [FromQuery] UrlQuery urlQuery, [FromQuery] int classId = -1)
        {
            TestResultQueryModel test = await _appQueries.GetTestUserInfo(testId);

            if (test == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                });
            List<UserResultQueryModel> results = await _appQueries.GetListTestResult(testId, classId, urlQuery);
            int total = await _appQueries.CountUserTestResult(testId, classId, urlQuery);
            test.ListResult = results;

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = new
                    {
                        total = total,
                        items = test
                    }
                }
            });
        }
        [HttpGet]
        [Route("get-user-test-point")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserTestPoint([FromQuery] int testId)
        {

            int userId = Convert.ToInt32(_httpContext.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            UserResultQueryModel userPoint = await _appQueries.GetUserTestResut(testId, userId);

            TestUserQueryModel test = await _appQueries.GetTestInfo(testId);

            if (test == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                });
            var res = new { test, userPoint };

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = res
                }
            });
        }
        [HttpGet]
        [Route("get-user-test-result-detail")]
        [CustomAuthorize(Allows = "Teacher,Admin")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserTestResultDetail([FromQuery] int testId, int userId, [FromQuery] UrlQuery urlQuery)
        {
            TestUserQueryModel test = await _appQueries.GetTestInfo(testId);

            if (test == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                });
            List<TestQuestionQueryModel> questions = await _appQueries.GetQuestionTest(userId, testId, urlQuery);
            foreach (TestQuestionQueryModel question in questions)
            {
                List<AnswerQueryModel> answers = await _appQueries.GetAnswerQuestion(question.QuestionId);
                List<TestQuestionResult> userChoose = await _appQueries.GetUserAnswer(question.Id);
                question.UserChoose = userChoose;
                question.ListAnswers = answers;
            }
            test.questions = questions;

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = test
                }
            });
        }
        [HttpGet]
        [Route("get-list-test-student")]
        [ProducesResponseType(typeof(Response<Pagination<TestUserQueryModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListTestStudent([FromQuery] UrlQuery urlQuery)
        {
            DateTime dateTime = DateTime.Now;
            List<TestUserQueryModel> tests = await _appQueries.GetListTestForStudent(dateTime, urlQuery);
            int total = await _appQueries.CountListTestStudent(dateTime, urlQuery);
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
                    Items = tests.ToList(),
                    Total = total
                }
            });
        }
        [HttpGet]
        [Route("get-list-test-create-by-self")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListTestCreateBySelf()
        {
            string userName = _httpContext.HttpContext.User.Identity.Name;
            List<ListTestCreate> ListTestCreate = await _appQueries.GetListTestCreateByUser(userName);
            if (ListTestCreate.Count() == 0)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                });

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = ListTestCreate
                }
            });
        }
        [HttpPost]
        [Route("check-pass-test")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CheckPassTest([FromBody] CheckPassTestCommand command)
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
        [Route("get-all-class")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllClass()
        {
            List<Class> classs = _userRepository.Classes.ToList();
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = classs
                }
            });
        }
        [HttpGet]
        [Route("get-student-class")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudentOfClass([FromQuery] int classId)
        {
            List<User> classs = await _appQueries.GetUserClass(classId);
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = classs
                }
            });
        }
    }
}
