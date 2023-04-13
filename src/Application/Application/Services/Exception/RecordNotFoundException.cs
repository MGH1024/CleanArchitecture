using System.Net;
using System.Runtime.Serialization;
using Utility;

namespace Application.Services.Exception;

[Serializable]
public class RecordNotFoundException : ServiceException
{
    public RecordNotFoundException()
    {
    }

    protected RecordNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public RecordNotFoundException(string message) : base(message)
    {
        ExceptionMessages = new string[1] { message };
    }

    public RecordNotFoundException(string message, ServiceException innerException) : base(message, innerException)
    {

    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
    public override string[] ExceptionMessages => new string[] { Messages.RecordNotFound };
    public override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
}
