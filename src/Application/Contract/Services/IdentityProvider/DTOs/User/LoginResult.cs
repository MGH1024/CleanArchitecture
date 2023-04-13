namespace Contract.Services.IdentityProvider.DTOs.User;

public class LoginResult
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? TokenValidDate { get; set; }
    public string ReturnUrl { get; set; }
}