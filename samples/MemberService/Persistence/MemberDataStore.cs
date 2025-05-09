using MemberService.Endpoints.Members;

namespace MemberService.Persistence;

internal sealed class MemberDataStore
{
    public int LastId { get; set; }

    public List<MemberEntity> Entities { get; init; } = [];

    public int GetNextId() => ++LastId;
}
