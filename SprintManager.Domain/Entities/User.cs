namespace SprintManager.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public User(string name, string email, string password) 
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name can't be null.", nameof(name));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email can't be null.", nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password can't be null.", nameof(password));

            if (name.Length > 255) throw new ArgumentException("Name can't exceed 255 characters.", nameof(name));
            if (email.Length > 255) throw new ArgumentException("Email can't exceed 255 characters.", nameof(email));
            if (password.Length < 12) throw new ArgumentException("Password can't have less than 12 characters.", nameof(password));
            if (password.Length > 64) throw new ArgumentException("Password can't have more than 64 characters.", nameof(password));

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = hashedPassword;
        }

        // Update user's name
        public void SetName(string name) 
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name can't be null.", nameof(name));
            if (name.Length > 255) throw new ArgumentException("Name can't exceed 255 characters.", nameof(name));

            Name = name; 
        }

        // Update user's email
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email can't be null.", nameof(email));
            if (email.Length > 255) throw new ArgumentException("Email can't exceed 255 characters.", nameof(email));

            Email = email;
        }

        // Update user's password
        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password can't be null.", nameof(password));
            if (password.Length < 12) throw new ArgumentException("A palavra-passe deve ter pelo menos 12 caracteres.", nameof(password));
            if (password.Length > 64) throw new ArgumentException("A palavra-passe não pode exceder 64 caracteres.", nameof(password));

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
