﻿namespace Contract.Services.IdentityProvider.DTOs.User;
public class Login
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
    public bool RememberMe { get; set; }
}