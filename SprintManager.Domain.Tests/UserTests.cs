using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_Constructor_WithValidData_CreatesUserSuccessfully()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            string passwordHash = user.PasswordHash;
        
            Assert.Equal("Daniel", user.Name);
            Assert.Equal("d@gmail.com", user.Email);
            Assert.True(user.VerifyPassword("abc123abc123"));
        }

        [Fact]
        public void SetName_UpdatesNameSuccessfully()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            user.SetName("Tiago");

            Assert.Equal("Tiago", user.Name);
        }

        [Fact]
        public void SetEmail_UpdatesEmailSuccessfully()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            user.SetEmail("t@gmail.com");

            Assert.Equal("t@gmail.com", user.Email);
        }
        
        [Fact]
        public void SetPassword_UpdatesPasswordSuccessfully()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            user.SetPassword("abc456abc456");

            string passwordHash = user.PasswordHash;

            Assert.True(user.VerifyPassword("abc456abc456"));
        }

        [Fact]
        public void VerifyName_ThrowsException_WhenNameIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => 
                new User("", "d@gmail.com", "abc123abc123")
            );

            Assert.Equal("Name can't be null or empty. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void VerifyName_ThrowsException_WhenNameIsTooLong()
        {
            string name = new string('D', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() => 
                new User(name, "d@gmail.com", "abc123abc123")
            );

            Assert.Equal($"Name is too long. (Max length '255') (Actual length '{name.Length}') (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void VerifyEmail_ThrowsException_WhenEmailIsNullEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new User("Daniel", "", "abc123abc123")
            );

            Assert.Equal("Email can't be null or empty. (Parameter 'email')", exception.Message);
        }

        [Fact]
        public void VerifyEmail_ThrowsException_WhenEmailIsTooLong()
        {
            string email = new string('d', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Daniel", email, "abc123abc123")
            );

            Assert.Equal($"Email is too long. (Max length '255') (Actual length '{email.Length}') (Parameter 'email')", exception.Message);
        }

        [Fact]
        public void VerifyPassword_ThrowsException_WhenPasswordIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new User("Daniel", "d@gmail.com", "")
            );

            Assert.Equal("Password can't be null or empty. (Parameter 'password')", exception.Message);
        }

        [Fact]
        public void VerifyPassword_ThrowsException_WhenPasswordIsTooShort()
        {
            string password = new string('a', 11);

            var exception = Assert.Throws<SprintManagerTooShortException>(() =>
                new User("Daniel", "d@gmail.com", password)
            );

            Assert.Equal($"Password is too short. (Min length '12') (Actual length '{password.Length}') (Parameter 'password')", exception.Message);
        }

        [Fact]
        public void VerifyPassword_ThrowsException_WhenPasswordIsTooLong()
        {
            string password = new string('a', 65);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Daniel", "d@gmail.com", password)
            );

            Assert.Equal($"Password is too long. (Max length '64') (Actual length '{password.Length}') (Parameter 'password')", exception.Message);
        }
    }
}