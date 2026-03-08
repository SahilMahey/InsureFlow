public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; } // BCrypt hashed password
    public string Role { get; set; } // Admin, Underwriter, Agent
}