using AutoMapper;
using Moq;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.Users;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Users;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.UserTests
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            // Initialize mocks for each test
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize hanlder injecting the mocks
            _handler = new CreateUserHandler(_mockUserRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_CreatesUserAndReturnsUserDTO()
        {
            var command = new CreateUserCommand
            {
                Name = "Daniel",
                Email = "d@gmail.com",
                Password = "abc123abc123"
            };

            var user = new User(command.Name, command.Email, command.Password);
            var userDto = new UserDTO { Id = user.Id, Name = user.Name, Email = user.Email };

            // Repository's Mock configuration
            _mockUserRepository.Setup(r => r.AddAsync(user));

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<UserDTO>(It.IsAny<User>())).Returns(userDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(userDto.Id, result.Id);
            Assert.Equal(userDto.Name, result.Name);
            Assert.Equal(userDto.Email, result.Email);
        }

        // Test exception throwing when request is null
        [Fact]
        public async Task VerifyRequest_ThrowsException_WhenRequestIsNull()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _handler.Handle(null!, CancellationToken.None)
            );

            Assert.Equal("request", exception.ParamName);
        }

        // Test exception throwing when user already exists
        [Fact]
        public async Task VerifyUserEmail_ThrowsException_WhenUserEmailAlreadyExists()
        {
            var command = new CreateUserCommand
            {
                Name = "Daniel",
                Email = "d@gmail.com",
                Password = "abc123abc123"
            };

            var user = new User(command.Name, command.Email, command.Password);

            _mockUserRepository.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(user);

            var exception = await Assert.ThrowsAsync<SprintManagerConflictException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User with email '{command.Email}' already exists.", exception.Message);
        }
    }
}
