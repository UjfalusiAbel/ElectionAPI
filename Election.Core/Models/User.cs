namespace Election.Core.Models
{
    public enum UserRole
    {
        Voter,
        Admin
    }

    public class User
    {
        public string CNP { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
