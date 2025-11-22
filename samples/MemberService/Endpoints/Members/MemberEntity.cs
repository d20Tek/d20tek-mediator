namespace MemberService.Endpoints.Members;

public sealed class MemberEntity(int id, string firstName, string lastName, string email, string? cellPhone = null)
{
    public int Id { get; private set; } = id;

    public string FirstName { get; } = firstName;

    public string LastName { get; } = lastName;

    public string Email { get; } = email;

    public string? CellPhone { get; } = cellPhone;

    public void SetId(int id) => Id = id;

    public static MemberEntity Create(int id, string firstName, string lastName, string email, string? cellPhone = null) =>
        new(id, firstName, lastName, email, cellPhone);

    public static MemberEntity Default => new(0, string.Empty, string.Empty, string.Empty);
}
