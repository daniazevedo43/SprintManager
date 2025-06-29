using SprintManager.Domain.Entities;

namespace SprintManager.Domain.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_Constructor_WithValidData_CreatesUserSuccessfully()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123");
        
            
        }
    }
}