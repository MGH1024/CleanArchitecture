using System.Net;
using System.Runtime.Serialization;
using Utility;

namespace Application.Services.Exception;


[Serializable]
public class CreateRecordException : ServiceException
{
    private readonly string _message;
    public CreateRecordException()
    {
    }

    protected CreateRecordException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CreateRecordException(string message) : base(message)
    {
        _message = message;
    }

    public CreateRecordException(string message, ServiceException innerException) : base(message, innerException)
    {
        _message = message;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }

    public override string[] ExceptionMessages => new string[] { _message +"، "+ Messages.BadDataFromClient };
    public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
}
