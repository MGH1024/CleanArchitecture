using FluentValidation.Results;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Utility;

namespace Application.Services.Exception;

[Serializable]
public class BadRequestException : ServiceException
{
    private readonly string _message;
    public BadRequestException()
    {
    }

    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BadRequestException(string message) : base(message)
    {
        ExceptionMessages = new string[1] { message };
    }

    public BadRequestException(string message, ServiceException innerException) : base(message, innerException)
    {

    }

    public BadRequestException(List<ValidationFailure> validationFailures)
    {
        var message = new StringBuilder();
        foreach (var validationFailure in validationFailures)
        {
            message.Append(string.Format($"PropertyName : {validationFailure.PropertyName} and ErrorMessage : {validationFailure.ErrorMessage} , "));
        }

        _message =  message.ToString();
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }

    public override string[] ExceptionMessages => new string[] { _message };
    public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
}
