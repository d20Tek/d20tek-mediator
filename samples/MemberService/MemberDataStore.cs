using MemberService.Endpoints.Members;

namespace MemberService;

internal sealed class MemberDataStore
{
    private static Failure<T> NotFoundError<T>(int id) where T : notnull =>
        Error.NotFound("MemberEntity.NotFound", $"Entity with id={id} not found.");

    private static Result<MemberEntity> EmailNotFound(string email) =>
        Result<MemberEntity>.Failure(
            Error.NotFound("Member.NotFound", $"Member entity with email={email} was not found."));

    public int LastId { get; set; }

    public List<MemberEntity> Entities { get; init; } = [];

    public int GetNextId() => ++LastId;

    public Result<MemberEntity> GetEntityById(int id) =>
        Entities.Where(x => x.Id == id)
                .Select(entity => (Result<MemberEntity>)entity)
                .DefaultIfEmpty(NotFoundError<MemberEntity>(id))
                .First();

    public Result<MemberEntity> GetEntityByEmail(string email) =>
        Entities.FirstOrDefault(p => p.Email == email) ?? EmailNotFound(email);
}
