using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Users;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Users;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.UserTests
{
    public class GetUserByIdHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetUserByIdHandler _handler;

        public GetUserByIdHandlerTests()
        {
            // Initialize mocks for each test
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize hanlder injecting the mocks
            _handler = new GetUserByIdHandler(_mockUserRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_GivenValidId_ReturnsUserDTO()
        {
            var userId = Guid.NewGuid();

            var query = new GetUserByIdQuery
            { 
                Id = userId,
            };

            var user = new User("Daniel", "d@gmail.com", "abc123abc123");
            var userDto = new UserDTO { Id = userId, Name = "Daniel", Email = "d@gmail.com" };
            
            _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<UserDTO>(It.IsAny<User>())).Returns(userDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(userId, result.Id);
            Assert.Equal("Daniel", result.Name);
            Assert.Equal("d@gmail.com", result.Email);
        }

        [Fact]
        public async Task VerifyRequest_ThrowsException_WhenRequestIsNull()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _handler.Handle(null!, CancellationToken.None)
            );

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNull()
        {
            var query = new GetUserByIdQuery
            {
                Id = Guid.NewGuid(),
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(query, CancellationToken.None)
            );

            Assert.Contains($"User with ID {query.Id} not found", exception.Message);
        }
    }
}