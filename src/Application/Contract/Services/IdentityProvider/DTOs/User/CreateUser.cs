namespace Contract.Services.IdentityProvider.DTOs.User;

public class CreateUser
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Password { get; set; }
}