namespace YourRest.Domain.Models;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public bool Enabled { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailVerified { get; set; }
    public Dictionary<string, string> Attributes { get; set; } // Custom attributes
    public List<Group> Groups { get; set; }
    public List<string> Roles { get; set; }
    public bool Totp { get; set; } // Two-factor authentication status
    public bool EmailVerification { get; set; }
}

