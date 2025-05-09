﻿namespace MemberService.Endpoints.Members;

public sealed class MemberEntity
{
    public int Id { get; private set; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public MemberEntity(int id, string firstName, string lastName, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public void SetId(int id) => Id = id;

    public static MemberEntity Create(int id, string firstName, string lastName, string email) =>
        new(id, firstName, lastName, email);

    public static MemberEntity Default => new(0, string.Empty, string.Empty, string.Empty);
}
