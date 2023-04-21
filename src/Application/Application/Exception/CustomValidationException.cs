using Contract.Services.Public.DTOs;
using System.Net;
using System.Runtime.Serialization;
using Utility;

namespace Application.Services.Exception;

[Serializable]
public class CustomValidationException : ServiceException
{
    public CustomValidationException()
    {
    }

    protected CustomValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CustomValidationException(string message) : base(message)
    {
        ExceptionMessages = new string[1] { message };
    }

    public CustomValidationException(string message, ServiceException innerException) : base(message, innerException)
    {

    }
    public CustomValidationException(List<ValidationMessage> validationErrors)
    {
        ValidationErrors = validationErrors ?? new List<ValidationMessage>();
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }

    public override string[] ExceptionMessages => !ValidationErrors.Any() ? null : GetValidationErrorMessages();
    public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

    public List<ValidationMessage> ValidationErrors { get; set; }

    private string[] GetValidationErrorMessages()
    {
        return ValidationErrors.Select(x => x.Message).ToArray();
    }
}

