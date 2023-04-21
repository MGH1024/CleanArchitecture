using System.Net;
using System.Runtime.Serialization;
using Utility;

namespace Application.Services.Exception;

[Serializable]
public class DuplicateInsertException : ServiceException
{
    public DuplicateInsertException()
    {
    }

    protected DuplicateInsertException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public DuplicateInsertException(string message) : base(message)
    {
        ExceptionMessages = new string[1] { message };
    }

    public DuplicateInsertException(string message, ServiceException innerException) : base(message, innerException)
    {

    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }

    public override string[] ExceptionMessages => new string[] { Messages.DuplicateInsert };
    public override HttpStatusCode HttpStatusCode => HttpStatusCode.Conflict;
}
