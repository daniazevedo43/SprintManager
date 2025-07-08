using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public User() 
        { 
        
        }

        public User(string name, string email, string password) 
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Name can't be null or empty.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email), "Email can't be null or empty.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password), "Password can't be null or empty.");

            if (name.Length > 255) throw new SprintManagerTooLongException("Name is too long.", 255, name.Length, nameof(name));
            if (email.Length > 255) throw new SprintManagerTooLongException("Email is too long.", 255, email.Length, nameof(email));
            if (password.Length < 12) throw new SprintManagerTooShortException("Password is too short.", 12, password.Length, nameof(password));
            if (password.Length > 64) throw new SprintManagerTooLongException("Password is too long.", 64, password.Length, nameof(password));

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = hashedPassword;
        }

        // Update user's name
        public void SetName(string name) 
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Name can't be null or empty.");
            if (name.Length > 255) throw new SprintManagerTooLongException("Name is too long.", 255, name.Length, nameof(name));

            Name = name; 
        }

        // Update user's email
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email), "Email can't be null or empty.");
            if (email.Length > 255) throw new SprintManagerTooLongException("Email is too long.", 255, email.Length, nameof(email));

            Email = email;
        }

        // Update user's password
        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password), "Password can't be null or empty.");
            if (password.Length < 12) throw new SprintManagerTooShortException("Password is too short.", 12, password.Length, nameof(password));
            if (password.Length > 64) throw new SprintManagerTooLongException("Password is too long.", 64, password.Length, nameof(password));

            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Check if the provided password matches the stored hash 
        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false; 

            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }
}
