﻿using AutoMapper;
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


namespace SmartCleanArchitecture.Application.Handler
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, PayloadResponse<UsersDto>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMessageProvider _messageProvider;
        private readonly IMapper _mapper;
        private readonly IMessageProducers _messageProducers;
        private IGetUserByCondition _getUserByCondition;

        public CreateUserCommandHandler(IRepositoryWrapper repositoryWrapper, IMessageProvider messageProvider, IMapper mapper, IMessageProducers messageProducers, IGetUserByCondition getUserByCondition)
        {
            _repositoryWrapper = repositoryWrapper;
            _messageProvider = messageProvider;
            _mapper = mapper;
            _getUserByCondition = getUserByCondition;
            _messageProducers = messageProducers;
        }
        public async Task<PayloadResponse<UsersDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var hashMethod = HashingAndSalting.GetHashingAndSalting;
            var newUser = new UsersDto();

            // using ClipsHashAndSalt to generate password hash and salt
            var hashResult = hashMethod.GenerateSaltedHash(request.Password);

            var existingUserEmail = await _getUserByCondition.GetUserByCondAsync(request.Email);
            var existingUserPhoneNo = await _getUserByCondition.GetUserByCondAsync(request.PhoneNo);

            if (existingUserEmail != null || existingUserPhoneNo != null)
            {
                return ResponseStatus<UsersDto>.Create<PayloadResponse<UsersDto>>(ResponseCodes.USER_EXIST, _messageProvider.GetMessage(ResponseCodes.USER_EXIST), newUser);
            }

            var user = new Users()
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNo = request.PhoneNo,
                UserName = request.UserName,
                PasswordHash = hashResult.Hash,
                PasswordSalt = hashResult.Salt,
                CreatedAt = DateTime.UtcNow,
            };

            await _repositoryWrapper.UserRepository.CreateAsync(user);
            await _repositoryWrapper.UserRepository.SaveAsync();

            newUser = _mapper.Map<UsersDto>(user);
            await _messageProducers.ProduceAsync<UsersDto>("User", newUser).ConfigureAwait(false);
            return ResponseStatus<UsersDto>.Create<PayloadResponse<UsersDto>>(ResponseCodes.SUCCESSFUL, _messageProvider.GetMessage(ResponseCodes.SUCCESSFUL), newUser);
        }
    }
}
