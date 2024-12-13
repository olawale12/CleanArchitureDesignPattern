using AutoMapper;
using ClipsHashAndSalt;
using MediatR;
using SmartCleanArchitecture.Application.Commands;
using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Application.Common.Responses;
using SmartCleanArchitecture.Domain.Entities.Dals;
using SmartCleanArchitecture.Domain.Entities.Dtos;
using SmartCleanArchitecture.Domain.Interfaces.IWrapper;
using KafkaLibrary;
using KafkaLibrary.KafkaProducer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCleanArchitecture.Application.Interfaces;
using SmartCleanArchitecture.Application.Factories.Interfaces;


namespace SmartCleanArchitecture.Application.Handler
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, PayloadResponse<UsersDto>>
    {

        private readonly IUserStrategyFactory _userStrategyFactory;
        private readonly ICountryService _countryService;

        public CreateUserCommandHandler(IUserStrategyFactory userStrategyFactory, ICountryService countryService)
        {
            _userStrategyFactory =  userStrategyFactory;
            _countryService = countryService;
        }
        public async Task<PayloadResponse<UsersDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var countryCode = _countryService.GetCountryCode();
            var userStrategyService = _userStrategyFactory.UserBaseOnStrategy(countryCode);
            return await userStrategyService.UserOnboarding(request).ConfigureAwait(false);
        }
    }
}
