﻿using MediatR;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.AdminCommands
{
    public class BlockUserCommand : IRequest<Response<ResponseDefault>>
    {
        public int userId { get; set; }
    }
}
