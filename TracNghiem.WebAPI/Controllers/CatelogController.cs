using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TracNghiem.WebAPI.Application.Commands.Categories;
using TracNghiem.WebAPI.Infrastructure.ModelQueries;
using TracNghiem.WebAPI.Infrastructure.Queries;
using TracNghiem.WebAPI.Infrastructure.Response;
using TracNghiem.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]
    [CustomAuthorize(Allows = "Teacher,Admin")]
    public class CatelogController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppQueries _appQueries;
        public CatelogController(IMediator mediator,
            IAppQueries appQueries)
        {
            _mediator = mediator;
            _appQueries = appQueries;
        }
        [HttpPost]
        [Route("add-category")]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddNewCategory([FromQuery]AddCategoryCommand command)
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
        [Route("get-all-category")]
        [ProducesResponseType(typeof(Response<Pagination<CategoryQueryModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseDefault>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCategory()
        {
            List<CategoryQueryModel> categories = await _appQueries.GetAllCategory();
            if (categories == null)
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB
                });
            return Ok(new Response<Pagination<CategoryQueryModel>>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new Pagination<CategoryQueryModel>()
                {
                    Items = categories,
                    Total = categories.Count()
                }
            });
        }
    }
}
