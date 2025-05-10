using D20Tek.Functional;
using D20Tek.Mediator;

namespace MemberService.Endpoints.Members;

internal sealed class DeleteMember
{
    public sealed record Command(int Id) : ICommand<Result<MemberResponse>>;
}
