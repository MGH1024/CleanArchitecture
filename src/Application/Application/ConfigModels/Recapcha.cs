namespace Application.ConfigModels;

public class Recaptcha
{
    public string ReCaptchaResponseUri { get; set; }
    public string ReCaptchaSiteKey { get; set; }
    public string ReCaptchaSecretKey { get; set; }
    public float MinimumRecaptchaValidationThreshold { get; set; }
}