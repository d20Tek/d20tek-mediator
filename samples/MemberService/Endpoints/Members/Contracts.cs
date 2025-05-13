namespace MemberService.Endpoints.Members;

internal sealed record MemberResponse(int Id, string FirstName, string LastName, string Email, string? CellPhone)
{
    public static MemberResponse Map(MemberEntity member) =>
        new(member.Id, member.FirstName, member.LastName, member.Email, member.CellPhone);

}

internal sealed record CreateMemberRequest(string FirstName, string LastName, string Email, string? CellPhone)
{
    public CreateMember.Command Map() => new(FirstName, LastName, Email, CellPhone);
}

internal sealed record UpdateMemberRequest(int Id, string FirstName, string LastName, string Email, string? CellPhone)
{
    public UpdateMember.Command Map(int id) => new(id, FirstName, LastName, Email, CellPhone);
}

