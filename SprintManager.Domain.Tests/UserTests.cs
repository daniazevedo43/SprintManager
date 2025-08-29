using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Tests
{
    public class UserTests
    {
        // Test user creation
        [Fact]
        public void User_Constructor_WithValidData_CreatesUserSuccessfully()
        {
            var user = new User("Daniel", "daniazevedo43", "d@gmail.com", "Abc123abc123!");

            Assert.Equal("Daniel", user.Name);
            Assert.Equal("daniazevedo43", user.UserName);
            Assert.Equal("d@gmail.com", user.Email);
        }

        // Test user's name change
        //[Fact]
        //public void SetName_UpdatesNameSuccessfully()
        //{
        //    var user = new User("Daniel", "d@gmail.com", "Abc123abc123!");

        //    user.SetName("Tiago");

        //    Assert.Equal("Tiago", user.UserName);
        //}

        // Test email change
        //[Fact]
        //public void SetEmail_UpdatesEmailSuccessfully()
        //{
        //    var user = new User("Daniel", "d@gmail.com", "Abc123abc123!");

        //    user.SetEmail("t@gmail.com");

        //    Assert.Equal("t@gmail.com", user.Email);
        //}

        // Test exception throwing when user is null or empty
        [Theory] 
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyName_ThrowsException_WhenNameIsNullOrEmpty(string name)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new User(name, "daniazevedo43", "d@gmail.com", "Abc123abc123!")
            );

            Assert.Equal("Name can't be null or empty. (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when name is too long
        [Fact]
        public void VerifyName_ThrowsException_WhenNameIsTooLong()
        {
            string name = new string('D', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() => 
                new User(name, "daniazevedo43", "d@gmail.com", "Abc123abc123!")
            );

            Assert.Equal($"Name is too long. (Max length '255') (Actual length '{name.Length}') (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when username is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyUsername_ThrowsException_WhenUsernameIsNullOrEmpty(string username)
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new User("Daniel", username, "d@gmail.com", "Abc123abc123!")
            );

            Assert.Equal("Username can't be null or empty. (Parameter 'username')", exception.Message);
        }

        // Test exception throwing when username has blank spaces
        [Theory]
        [InlineData("dani azevedo")]
        [InlineData("dani azevedo 43")]
        public void VerifyUsername_ThrowsException_WhenUsernameHasBlankSpaces(string username)
        {
            var exception = Assert.Throws<SprintManagerInvalidUsernameException>(() =>
                new User("Daniel", username, "d@gmail.com", "Abc123abc123!")
            );

            Assert.Equal("Username can't have blank spaces.", exception.Message);
        }

        // Test exception throwing when username is too long
        [Fact]
        public void VerifyUsername_ThrowsException_WhenUsernameIsTooLong()
        {
            string username = new string('d', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Daniel", username, "d@gmail.com", "Abc123abc123!")
            );

            Assert.Equal($"UserName is too long. (Max length '255') (Actual length '{username.Length}') (Parameter 'username')", exception.Message);
        }

        // Test exception throwing when email is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyEmail_ThrowsException_WhenEmailIsNullEmpty(string email)
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new User("Daniel", "daniazevedo43", email, "Abc123abc123!")
            );

            Assert.Equal("Email can't be null or empty. (Parameter 'email')", exception.Message);
        }

        // Test exception throwing when email is too long
        [Fact]
        public void VerifyEmail_ThrowsException_WhenEmailIsTooLong()
        {
            string email = new string('d', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Daniel", "daniazevedo43", email, "Abc123abc123!")
            );

            Assert.Equal($"Email is too long. (Max length '255') (Actual length '{email.Length}') (Parameter 'email')", exception.Message);
        }

        // Test exception throwing when password is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyPassword_ThrowsException_WhenPasswordIsNullOrEmpty(string password)
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new User("Daniel", "daniazevedo43", "d@gmail.com", password)
            );

            Assert.Equal("Password can't be null or empty. (Parameter 'password')", exception.Message);
        }

        // Test exception throwing when password needs an uppercase letter
        [Theory]
        [InlineData("abc123abc123!")]
        public void VerifyPassword_ThrowsException_WhenPasswordNeedsUppercaseLetter(string password)
        {
            var exception = Assert.Throws<SprintManagerPasswordRuleException>(() =>
                new User("Daniel", "daniazevedo43", "d@gmail.com", password)
            );

            Assert.Equal($"Password needs to have at least one uppercase letter.", exception.Message);
        }

        // Test exception throwing when password needs a number
        [Theory]
        [InlineData("Abcabcabcabc!")]
        public void VerifyPassword_ThrowsException_WhenPasswordNeedsNumber(string password)
        {
            var exception = Assert.Throws<SprintManagerPasswordRuleException>(() =>
                new User("Daniel", "daniazevedo43", "d@gmail.com", password)
            );

            Assert.Equal($"Password needs to have at least one number.", exception.Message);
        }

        // Test exception throwing when password needs a special character
        [Theory]
        [InlineData("Abc123abc123a")]
        public void VerifyPassword_ThrowsException_WhenPasswordNeedsSpecialCharacter(string password)
        {
            var exception = Assert.Throws<SprintManagerPasswordRuleException>(() =>
                new User("Daniel", "daniazevedo43", "d@gmail.com", password)
            );

            Assert.Equal($"Password needs to have at least one special character.", exception.Message);
        }

        // Test exception throwing when password is too short
        [Theory]
        [InlineData("Abc123!")]
        public void VerifyPassword_ThrowsException_WhenPasswordIsTooShort(string password)
        {
            var exception = Assert.Throws<SprintManagerTooShortException>(() =>
                new User("Daniel", "daniazevedo43", "d@gmail.com", password)
            );

            Assert.Equal($"Password is too short. (Min length '12') (Actual length '{password.Length}') (Parameter 'password')", exception.Message);
        }

        // Test exception throwing when password is too long
        [Fact]
        public void VerifyPassword_ThrowsException_WhenPasswordIsTooLong()
        {
            string password = "";

            for (int i = 0; i < 65; i++)
            {
                password += "Abc123!";
            }

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Daniel", "daniazevedo43", "d@gmail.com", password)
            );

            Assert.Equal($"Password is too long. (Max length '64') (Actual length '{password.Length}') (Parameter 'password')", exception.Message);
        }
    }
}