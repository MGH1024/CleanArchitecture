namespace Contract.Services.IdentityProvider.DTOs.User;

public class CreateUserDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}