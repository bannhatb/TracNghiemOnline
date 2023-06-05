using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Application.Commands.Categories;
using TracNghiem.WebAPI.Application.Commands.QuestionCommands;
using TracNghiem.WebAPI.Infrastructure.ModelQueries;
using TracNghiem.WebAPI.Infrastructure.Queries;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]
    public class QuestionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppQueries _appQueries;
        private readonly IQuestionRepository _questionRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IExamRepository _examRepository;
        public QuestionController(IMediator mediator,
            IAppQueries appQueries,
            IQuestionRepository questionRepository,
            IHttpContextAccessor httpContext,
            IExamRepository examRepository)
        {
            _mediator = mediator;
            _appQueries = appQueries;
            _questionRepository = questionRepository;
            _examRepository = examRepository;
            _httpContext = httpContext;
        }

        [HttpPost]
        [Route("add-question-category")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddQuestionCategory(AddQuestionCategoryCommand command)
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
        [Route("add-exam-category")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddExamCategory(AddExamCategoryCommand command)
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
        [Route("get-question-detail")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> QuestionDetail([FromQuery] int questionId)
        {
            QuestionDetailQueryModel question = await _appQueries.GetQuestionDetailById(questionId);
            //question.ListCategories = await _appQueries.GetCategoryQuestion(questionId);
            question.Level = await _appQueries.GetNameLevelQuestion(questionId);
            question.ListAnswers = await _appQueries.GetAnswerQuestion(questionId);
            if (question == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB
                });
            return Ok(new Response<QuestionDetailQueryModel>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = question
            });
        }

        [HttpPost]
        [Route("add-question")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddQuestion(AddQuestionCommand command)
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
        [Route("update-question")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateQuestion(UpdateQuestionCommand command)
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
        [Route("add-question-to-exam")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddQuestionToExam(AddQuestionExamCommand command)
        {
            if (command == null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);

            if (result is not null && result.State)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete]
        [Route("delete-question/{id}")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            if (id.ToString() is null)
            {
                return BadRequest(null);
            }
            string userName = _httpContext.HttpContext.User.Identity.Name.ToString();

            var question = await _questionRepository.Questions.FirstOrDefaultAsync(x => x.Id == id);
            if (question is null)
            {
                return NotFound(ErrorCode.NotFound);
            }
            _questionRepository.Delete(question);

            var result = await _questionRepository.UnitOfWork.SaveAsync();

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Xóa thành công question"
                    }
                });
            }

            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Lỗi khi xóa question"
                }
            });
        }

        [HttpDelete]
        [Route("delete-question-exam")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int examId, int questionId)
        {
            if (examId.ToString() is null && questionId.ToString() is null)
            {
                return BadRequest(null);
            }
            string userName = _httpContext.HttpContext.User.Identity.Name.ToString();

            var exam = await _examRepository.Exams.FirstOrDefaultAsync(x => x.CreateBy == userName && x.Id == examId);
            if (exam is null)
            {
                return NotFound(ErrorCode.NotFound);
            }
            var question = await _questionRepository.Questions.FirstOrDefaultAsync(x => x.Id == questionId);
            if (question is null)
            {
                return NotFound(ErrorCode.NotFound);
            }
            var questionExam = new QuestionExam();
            questionExam.QuestionId = questionId;
            questionExam.ExamId = examId;
            _questionRepository.Delete(questionExam);

            var result = await _questionRepository.UnitOfWork.SaveAsync();

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Xóa thành công question exam"
                    }
                });
            }

            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Lỗi khi xóa question"
                }
            });
        }

        [HttpGet]
        [Route("get-list-id-question")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListIdQuestion([FromQuery] int categoryId, [FromQuery] int questionCount, [FromQuery] int levelId)
        {
            var listIdQuestion = _questionRepository.GetListIdQuestionByCategoryId(categoryId, questionCount, levelId);
            if (listIdQuestion == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB
                });

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault
                {
                    Data = listIdQuestion
                }

            });
        }



    }
}
