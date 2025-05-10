namespace MemberService.Endpoints.Members;

internal sealed record MemberResponse(int Id, string FirstName, string LastName, string Email)
{
    public static MemberResponse Map(MemberEntity member) =>
        new(member.Id, member.FirstName, member.LastName, member.Email);

}

internal sealed record CreateMemberRequest(string FirstName, string LastName, string Email)
{
    public CreateMember.Command Map() => new(FirstName, LastName, Email);
}

