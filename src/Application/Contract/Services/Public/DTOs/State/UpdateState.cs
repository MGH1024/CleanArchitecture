using Contract.Services.Public.DTOs.State.Base;

namespace Contract.Services.Public.DTOs.State;

public class UpdateState : StateDto
{
    public int Id { get; set; }
    public int Order { get; set; }
}