using Contract.Services.Public.DTOs.State.Base;

namespace Contract.Services.Public.DTOs.State;

public class StateDetail:StateDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public string CreatedDate { get; set; }
}