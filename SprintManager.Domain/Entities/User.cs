using Microsoft.AspNetCore.Identity;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }

        public User() 
        { 
        
        }

        public User(string name, string userName, string email, string password) 
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Name can't be null or empty.");
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "Username can't be null or empty.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email), "Email can't be null or empty.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password), "Password can't be null or empty.");

            if (userName.Contains(" ")) throw new SprintManagerInvalidUsernameException("Username can't have blank spaces.");

            if (name.Length > 255) throw new SprintManagerTooLongException("Name is too long.", 255, name.Length, nameof(name));
            if (userName.Length > 255) throw new SprintManagerTooLongException("UserName is too long.", 255, name.Length, nameof(name));
            if (email.Length > 255) throw new SprintManagerTooLongException("Email is too long.", 255, email.Length, nameof(email));
            if (password.Length < 12) throw new SprintManagerTooShortException("Password is too short.", 12, password.Length, nameof(password));
            if (password.Length > 64) throw new SprintManagerTooLongException("Password is too long.", 64, password.Length, nameof(password));

            Name = name;
            UserName = userName;
            Email = email;
        }

        // Update userName
        //public void SetName(string userName) 
        //{
        //    if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "Name can't be null or empty.");
        //    if (userName.Length > 255) throw new SprintManagerTooLongException("Name is too long.", 255, userName.Length, nameof(userName));

        //    UserName = userName; 
        //}

        // Update user's email
        //public void SetEmail(string email)
        //{
        //    if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email), "Email can't be null or empty.");
        //    if (email.Length > 255) throw new SprintManagerTooLongException("Email is too long.", 255, email.Length, nameof(email));

        //    Email = email;
        //}
    }
}