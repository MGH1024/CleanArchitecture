namespace Contract.Services.Public.DTOs;

public class LogProperties
{
    public string CorrelationId { get; set; }
    public string LogMessage { get; set; }
    public string MessageBody { get; set; }
    public string ApplicationName { get; set; }
    public string Response { get; set; }
    public string Url { get; set; }
    public string Exception { get; set; }

    //api param
    public string ClientIP { get; set; }
    public string Params { get; set; }
    public string Headers { get; set; }
}
