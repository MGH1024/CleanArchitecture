using System.Net;

namespace Contract.Services.Public.DTOs
{
    [Serializable]
    public class ApiResult<T>
    {
        public ApiResult()
        {
            Messages = new List<string>();
            Summaries = new List<string>();
            ValidationMessages = new List<ValidationMessage>();
        }
        public T Data { get; set; }
        public List<string> Messages { get; set; }
        public List<string> Summaries { get; set; }
        public List<ValidationMessage> ValidationMessages { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }
    }

    [Serializable]
    public class ValidationMessage
    {
        public ValidationMessage(string propName, string message)
        {
            PropName = propName;
            Message = message;
        }
        public string PropName { get; set; }
        public string Message { get; set; }
    }
}
