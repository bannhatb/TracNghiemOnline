using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using TracNghiem.WebAPI.Infrastructure.Exceptions;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> logger;
        public HttpGlobalExceptionFilter(
            ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);
            if (context.Exception.GetType() == typeof(DomainException))
            {
                if (context.Exception.InnerException != null)
                {
                    if (context.Exception.InnerException.GetType() == typeof(FluentValidation.ValidationException))
                    {
                        List<ValidatorError> errors = new List<ValidatorError>();
                        var exception = context.Exception.InnerException as FluentValidation.ValidationException;
                        foreach (var error in exception.Errors)
                        {
                            errors.Add(new ValidatorError()
                            {
                                FieldName = error.PropertyName,
                                Message = error.ErrorMessage
                            });
                        }
                        context.Result = new BadRequestObjectResult(new Response<List<ValidatorError>>()
                        {
                            State = false,
                            Message = ErrorCode.ValidateError,
                            Result = errors
                        });
                    }
                    else
                    {
                        context.Result = new BadRequestObjectResult(new Response<ResponseDefault>()
                        {
                            State = false,
                            Result = new ResponseDefault()
                            {
                                Data = context.Exception.InnerException.Message
                            },
                            Message = ErrorCode.BadRequest
                        });
                    }
                }
                else
                {
                    context.Result = new BadRequestObjectResult(new Response<ResponseDefault>()
                    {
                        State = false,
                        Result = new ResponseDefault()
                        {
                            Data = context.Exception.Message
                        },
                        Message = ErrorCode.BadRequest
                    });
                    //
                }
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                string message = context.Exception.InnerException != null ? context.Exception.InnerException.Message : context.Exception.Message;
                context.Result = new BadRequestObjectResult(new Response<ResponseDefault>()
                {
                    State = false,
                    Result = new ResponseDefault()
                    {
                        Data = message
                    },
                    Message = ErrorCode.BadRequest
                });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //
            }
            context.ExceptionHandled = true;
        }
    }
}
