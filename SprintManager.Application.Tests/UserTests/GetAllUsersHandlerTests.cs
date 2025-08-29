using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Users;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Users;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.UserTests
{
    public class GetAllUsersHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllUsersHandler _handler;

        public GetAllUsersHandlerTests()
        {
            // Initialize mocks for each test
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetAllUsersHandler(_mockUserRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_ReturnsAllUsers()
        {
            var query = new GetAllUsersQuery();

            var users = new List<User>
            {
                new User("Daniel", "daniazevedo43", "d@gmail.com", "Abc123abc123!"),
                new User("Hugo", "daniazevedo43", "h@gmail.com", "def456def456"),
            };

            var usersDTOs = new List<UserDTO>
            {
                new UserDTO 
                { 
                    Id = users[0].Id,
                    Name = users[0].Name,
                    UserName = users[0].UserName,
                    Email = users[0].Email 
                },
                new UserDTO
                {
                    Id = users[1].Id,
                    Name = users[1].Name,
                    UserName = users[0].UserName,
                    Email = users[1].Email
                }
            };

            // Repository's Mock configuration
            _mockUserRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<UserDTO>>(users)).Returns(usersDTOs);

            var result = await _handler.Handle(query, CancellationToken.None);

            for(int i = 0; i < usersDTOs.Count; i++)
            {
                Assert.Equal(usersDTOs[i].Id, result[i].Id);
                Assert.Equal(usersDTOs[i].Name, result[i].Name);
                Assert.Equal(usersDTOs[i].UserName, result[i].UserName);
                Assert.Equal(usersDTOs[i].Email, result[i].Email);
            }

            // Ensure GetAllAsync was called exactly once.
            _mockUserRepository.Verify(r => r.GetAllAsync(), Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<UserDTO>>(users), Times.Once);
        }
    }
}