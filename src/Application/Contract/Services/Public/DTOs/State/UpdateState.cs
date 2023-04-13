namespace Contract.Services.Public.DTOs.State;

public class UpdateState
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Code { get; set; }
    public int Order { get; set; }
}