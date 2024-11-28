using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCleanArchitecture.Api.Controllers;
using SmartCleanArchitecture.Application.Commands;
using SmartCleanArchitecture.Application.Common.Responses;
using SmartCleanArchitecture.Domain.Entities.Dtos;

namespace CleanArchitechtureDesignPattern.Test
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task CreateSSOUser_ShouldReturnOkResult_WithBasicResponse()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();

            var createUserCommand = new CreateUserCommand
            {
                UserName = "TestUser",
                PhoneNo = "1234567890",
                FirstName = "Test",
                LastName = "User",
                Dob = "1990-01-01",
                Password = "SecurePassword",
                Email = "testuser@example.com"
            };

            var expectedPayload = new UsersDto
            {
                UserName = createUserCommand.UserName,
                Email = createUserCommand.Email,
                PhoneNo = createUserCommand.PhoneNo
            };

            var expectedResponse = ResponseStatus<UsersDto>.Create<PayloadResponse<UsersDto>>(
                "200",
                "User created successfully",
                expectedPayload
            );

            mockMediator
                .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), default))
                .ReturnsAsync(expectedResponse);

            // Using reflection to inject the dependency if UsersController has a private _mediator field
            var controller = new UsersController(); // Default constructor
            var mediatorField = typeof(UsersController)
                .GetField("_mediator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (mediatorField != null)
            {
                mediatorField.SetValue(controller, mockMediator.Object);
            }

            // Act
            var result = await controller.CreateSSOUser(createUserCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<PayloadResponse<UsersDto>>(okResult.Value);

            Assert.True(actualResponse.IsSuccessful);
            Assert.Equal(expectedResponse.Response.Code, actualResponse.Response.Code);
            Assert.Equal(expectedResponse.Response.Description, actualResponse.Response.Description);
            Assert.Equal(expectedPayload.UserName, actualResponse.Response.Data.UserName);
            Assert.Equal(expectedPayload.Email, actualResponse.Response.Data.Email);
            Assert.Equal(expectedPayload.PhoneNo, actualResponse.Response.Data.PhoneNo);

            mockMediator.Verify(m => m.Send(It.Is<CreateUserCommand>(c =>
                c.UserName == createUserCommand.UserName &&
                c.Email == createUserCommand.Email
            ), default), Times.Once);
        }
    }
}