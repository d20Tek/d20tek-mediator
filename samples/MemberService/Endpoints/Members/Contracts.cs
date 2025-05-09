namespace MemberService.Endpoints.Members;

internal record MemberResponse(int Id, string FirstName, string LastName, string Email)
{
    public static MemberResponse Map(MemberEntity member) =>
        new(member.Id, member.FirstName, member.LastName, member.Email);
}