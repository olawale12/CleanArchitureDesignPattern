using MediatR;
using SmartCleanArchitecture.Domain.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Queries
{
    public class GetUserQuery: IRequest<UsersDto>
    {
        public string UserId { get; set; }
    }
}
