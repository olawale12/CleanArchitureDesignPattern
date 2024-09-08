using MediatR;
using SmartCleanArchitecture.Application.Common.Responses;
using SmartCleanArchitecture.Domain.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Commands
{
    public class CreateUserCommand: IRequest<PayloadResponse<UsersDto>>
    {
        public  string UserName { get; set; }
        public  string PhoneNo { get; set; }
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  string Dob { get; set; }
        public  string Password { get; set; }
        public  string Email { get; set; }
    }
}
