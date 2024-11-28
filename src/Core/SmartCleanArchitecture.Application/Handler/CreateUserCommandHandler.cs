using AutoMapper;
using ClipsHashAndSalt;
using MediatR;
using SmartCleanArchitecture.Application.Commands;
using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Application.Common.Responses;
using SmartCleanArchitecture.Domain.Entities.Dals;
using SmartCleanArchitecture.Domain.Entities.Dtos;
using SmartCleanArchitecture.Domain.Interfaces.IWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Configuration;

namespace SmartCleanArchitecture.Application.Handler
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, PayloadResponse<UsersDto>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMessageProvider _messageProvider;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly OracleConnection _connection;

        public CreateUserCommandHandler(IRepositoryWrapper repositoryWrapper, IMessageProvider messageProvider, IMapper mapper, IConfiguration configuration)
        {
            _repositoryWrapper = repositoryWrapper;
            _messageProvider = messageProvider;
            _mapper = mapper;
            _configuration = configuration;
            _connection = new OracleConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<PayloadResponse<UsersDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var hashMethod = HashingAndSalting.GetHashingAndSalting;
            var userMethod = new GetUserByCondition(_configuration);
            var newUser = new UsersDto();

            // using ClipsHashAndSalt to generate password hash and salt
            var hashResult = hashMethod.GenerateSaltedHash(request.Password);

            var existingUserEmail = await userMethod.GetUserByCondAsync(request.Email);
            var existingUserPhoneNo = await userMethod.GetUserByCondAsync(request.PhoneNo);

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
            //_connection.q
            //await _repositoryWrapper.UserRepository.CreateAsync(user);
            //await _repositoryWrapper.UserRepository.SaveAsync();

            using (OracleConnection connection = new OracleConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                string insertSql = "insert into Users (DOB,EMAIL, FIRSTNAME, LASTNAME, PHONENO, USERNAME, PASSWORDHASH, PASSWORDSALT, CREATEDAT, LASTLOGIN, ISDELETED) values (:DOB, :EMAIL, :FIRSTNAME, :LASTNAME, :PHONENO, :USERNAME, :PASSWORDHASH, :PASSWORDSALT, :CREATEDAT, :LASTLOGIN, :ISDELETED)";

            using (OracleCommand command = new OracleCommand(insertSql, connection))
            {
                command.Parameters.Add(new OracleParameter("DOB", request.Dob));
                command.Parameters.Add(new OracleParameter("EMAIL", request.Email));
                command.Parameters.Add(new OracleParameter("FIRSTNAME", request.FirstName));
                command.Parameters.Add(new OracleParameter("LASTNAME", request.LastName));
                command.Parameters.Add(new OracleParameter("PHONENO", request.PhoneNo));
                command.Parameters.Add(new OracleParameter("USERNAME", request.UserName));
                command.Parameters.Add(new OracleParameter("PASSWORDHASH", hashResult.Hash));
                command.Parameters.Add(new OracleParameter("PASSWORDSALT", hashResult.Salt));
                command.Parameters.Add(new OracleParameter("CREATEDAT", DateTime.UtcNow));
                command.Parameters.Add(new OracleParameter("LASTLOGIN", DateTime.UtcNow));
                command.Parameters.Add(new OracleParameter("ISDELETED", 1));

                int rowsAffected = await command.ExecuteNonQueryAsync();

                Console.WriteLine($"Rows inserted: {rowsAffected}");
            }
        }

            newUser = _mapper.Map<UsersDto>(user);

            return ResponseStatus<UsersDto>.Create<PayloadResponse<UsersDto>>(ResponseCodes.SUCCESSFUL, _messageProvider.GetMessage(ResponseCodes.SUCCESSFUL), newUser);
        }
    }
}
