using MediatR;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.Categories
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Response<ResponseDefault>>
    {
        //private readonly IRepository<Category> _cateRepository;
        private readonly ICategoryRepository _cateRepository;
        public AddCategoryCommandHandler(ICategoryRepository cateRepository)
        {
            _cateRepository = cateRepository;
        }
        public async Task<Response<ResponseDefault>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = new Category();
            category.CategoryName = request.CategoryName;
            _cateRepository.Add(category);
            int result = await _cateRepository.UnitOfWork.SaveAsync(cancellationToken);
            if(result > 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = category.Id.ToString()
                    }
                };
            }
            return new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Excute Db error"
                }
            };
        }
    }
}
