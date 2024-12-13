using SmartCleanArchitecture.Application.Commands;
using SmartCleanArchitecture.Application.Common.Responses;
using SmartCleanArchitecture.Domain.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Factories.Abstracts
{
    public abstract class UserStrategyService
    {
        public abstract Task<PayloadResponse<UsersDto>> UserOnboarding(CreateUserCommand request);
    }
}
