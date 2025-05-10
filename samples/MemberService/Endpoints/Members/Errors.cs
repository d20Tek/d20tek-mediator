namespace MemberService.Endpoints.Members;

public static class Errors
{
    public static Failure<MemberEntity> AlreadyExists(int id) =>
        Error.Conflict("MemberEntity.AlreadyExists", $"Entity with id={id} already exists.");

    public static Result<MemberEntity> EmailNotFound(string email) =>
        Result<MemberEntity>.Failure(
            Error.NotFound("MemberEntity.NotFound", $"Member entity with email={email} was not found."));

    public static Error FirstNameRequired =
        Error.Validation("Member.FirstName.Required", "Member first name is required.");

    public static Error LastNameRequired =
        Error.Validation("Member.Last.Required", "Member last name is required.");

    public static Error EmailRequired =
        Error.Validation("Member.Email.Required", "Member email is required.");

    public static Error EmailInvalid =
        Error.Validation("Member.Email.Invalid", "Member email is not the expected format (name@company.com).");

    public static Error IdInvalid =
        Error.Validation("Member.Id", "Member id must be valid and greater than 0.");
}
