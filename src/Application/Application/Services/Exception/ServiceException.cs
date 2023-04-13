using System.Net;
using System.Runtime.Serialization;

namespace Application.Services.Exception;

[Serializable]
public class ServiceException : System.Exception
{
    public ServiceException()
    {
    }

    protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ServiceException(string message) : base(message)
    {
        ExceptionMessages = new string[1] { message };
    }

    public ServiceException(string message, ServiceException innerException) : base(message, innerException)
    {

    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }

    public virtual string[] ExceptionMessages { get; set; }
    public virtual HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.UnprocessableEntity;
}
