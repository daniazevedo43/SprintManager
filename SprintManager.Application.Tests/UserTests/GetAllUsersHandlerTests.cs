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

            // Initialize hanlder injecting the mocks
            _handler = new GetAllUsersHandler(_mockUserRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_ReturnsAllUsers()
        {
            var query = new GetAllUsersQuery();

            var users = new List<User>
            {
                new User("Daniel", "d@gmail.com", "abc123abc123"),
                new User("Hugo", "h@gmail.com", "def456def456"),
            };

            var usersDTOs = new List<UserDTO>
            {
                new UserDTO 
                { 
                    Id = users[0].Id, 
                    Name = users[0].Name, 
                    Email = users[0].Email 
                },
                new UserDTO
                {
                    Id = users[1].Id,
                    Name = users[1].Name,
                    Email = users[1].Email
                }
            };

            // Repository's Mock configuration
            _mockUserRepository.Setup(r => r.GetAllAsync());

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<UserDTO>>(It.IsAny<User>())).Returns(usersDTOs);

            var result = await _handler.Handle(query, CancellationToken.None);

            for(int i = 0; i < usersDTOs.Count; i++)
            {
                Assert.Equal(usersDTOs[i].Id, result[i].Id);
                Assert.Equal(usersDTOs[i].Name, result[i].Name);
                Assert.Equal(usersDTOs[i].Email, result[i].Email);
            }
        }
    }
}
