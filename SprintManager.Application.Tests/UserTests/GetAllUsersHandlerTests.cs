using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Users;
using SprintManager.Application.Queries.Users;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.UserTests
{
    public class GetAllUsersHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllUsersHandler _handler;

        public GetAllUsersHandlerTests()
        {
            // Create mocks for UserManager constructor's dependencies
            var mockUserStore = new Mock<IUserStore<User>>();
            var mockOptions = new Mock<IOptions<IdentityOptions>>();
            var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            var mockUserValidator = new List<IUserValidator<User>>
            {
                new Mock<IUserValidator<User>>().Object
            };
            var mockPasswordValidator = new List<IPasswordValidator<User>>
            {
                new Mock<IPasswordValidator<User>>().Object
            };
            var mockLookupNormalizer = new Mock<ILookupNormalizer>();
            var mockErrors = new Mock<IdentityErrorDescriber>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockLogger = new Mock<ILogger<UserManager<User>>>();

            // Initialize mock for each test
            _mockUserManager = new Mock<UserManager<User>>(
                mockUserStore.Object,
                mockOptions.Object,
                mockPasswordHasher.Object,
                mockUserValidator,
                mockPasswordValidator,
                mockLookupNormalizer.Object,
                mockErrors.Object,
                mockServiceProvider.Object,
                mockLogger.Object
            );
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetAllUsersHandler(
                _mockUserManager.Object, 
                _mockMapper.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_ReturnsAllUsers()
        {
            var query = new GetAllUsersQuery();

            var users = new List<User>
            {
                new User("Test", "test", "test@gmail.com", "Test123test123!"),
                new User("Test 2", "test2", "test@gmail.com", "Test456test456!"),
            };

            var usersDtos = new List<UserDto>
            {
                new UserDto
                { 
                    Id = users[0].Id,
                    Name = users[0].Name,
                    UserName = users[0].UserName,
                    Email = users[0].Email 
                },
                new UserDto
                {
                    Id = users[1].Id,
                    Name = users[1].Name,
                    UserName = users[1].UserName,
                    Email = users[1].Email
                }
            };

            var mockUsers = users.BuildMock();
            
            _mockUserManager.Setup(m => m.Users).Returns(mockUsers);

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<UserDto>>(users)).Returns(usersDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            for(int i = 0; i < usersDtos.Count; i++)
            {
                Assert.Equal(usersDtos[i].Id, result[i].Id);
                Assert.Equal(usersDtos[i].Name, result[i].Name);
                Assert.Equal(usersDtos[i].UserName, result[i].UserName);
                Assert.Equal(usersDtos[i].Email, result[i].Email);
            }

            // Ensure GetAllAsync was called exactly once.
            _mockUserManager.Verify(m => m.Users, Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<UserDto>>(users), Times.Once);
        }
    }
}