using D20Tek.Functional;
using D20Tek.Functional.Async;
using D20Tek.LowDb;
using D20Tek.Mediator;
using System.Text.RegularExpressions;

namespace MemberService.Endpoints.Members;

internal sealed class CreateMember
{
    public sealed record Command(string FirstName, string LastName, string Email)
        : ICommand<Result<MemberResponse>>;

    public sealed class Handler : ICommandHandlerAsync<Command, Result<MemberResponse>>
    {
        private static Failure<MemberEntity> AlreadyExistsError(int id) =>
            Error.Conflict("MemberEntity.AlreadyExists", $"Entity with id={id} already exists.");

        private readonly LowDbAsync<MemberDataStore> _db;

        public Handler(LowDbAsync<MemberDataStore> db) => _db = db;

        public async Task<Result<MemberResponse>> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            try
            {
                var store = await _db.Get();
                return await Validator.Validate(command)
                                      .Bind(c => CreateEntity(c, store))
                                      .MapAsync(entity => SaveEntity(entity, store));
            }
            catch (Exception ex)
            {
                return Result<MemberResponse>.Failure(ex);
            }
        }

        private Result<MemberEntity> CreateEntity(Command command, MemberDataStore store)
        {
            var newId = store.GetNextId();
            return store.Entities.Any(x => x.Id == newId) ? 
                AlreadyExistsError(newId) : 
                MemberEntity.Create(newId, command.FirstName, command.LastName, command.Email);
        }

        private async Task<MemberResponse> SaveEntity(MemberEntity entity, MemberDataStore store)
        {
            store.Entities.Add(entity);
            await _db.Write();
            return MemberResponse.Map(entity);
        }
    }

    public static class Validator
    {
        public static Result<Command> Validate(Command command) =>
            ValidationErrors.Create()
                .AddIfError(() => string.IsNullOrEmpty(command.FirstName), "Member.FirstName.Required", "Member first name is required.")
                .AddIfError(() => string.IsNullOrEmpty(command.LastName), "Member.Last.Required", "Member last name is required.")
                .AddIfError(() => string.IsNullOrEmpty(command.Email), "Member.Email.Required", "Member email is required.")
                .AddIfError(() => !IsValidEmailFormat(command.Email), "Member.Email.Invalid", "Member email is not the expected format (name@company.com).")
                .Map(() => command);

        private static readonly Regex _emailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool IsValidEmailFormat(string text) => _emailRegex.IsMatch(text);
    }
}
