using D20Tek.Functional;
using D20Tek.Functional.Async;
using D20Tek.LowDb;
using D20Tek.Mediator;
using MemberService.Common;

namespace MemberService.Endpoints.Members;

internal sealed class UpdateMember
{
    public sealed record Command(int Id, string FirstName, string LastName, string Email)
        : ICommand<Result<MemberResponse>>;

    public sealed class Handler : ICommandHandlerAsync<Command, Result<MemberResponse>>
    {
        private readonly LowDbAsync<MemberDataStore> _db;

        public Handler(LowDbAsync<MemberDataStore> db) => _db = db;

        public async Task<Result<MemberResponse>> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            try
            {
                var store = await _db.Get();
                return await Validator.Validate(command)
                                      .Bind(c => store.GetEntityById(command.Id))
                                      .MapAsync(entity => UpdateEntity(entity, command, store));
            }
            catch (Exception ex)
            {
                return Result<MemberResponse>.Failure(ex);
            }
        }

        private async Task<MemberResponse> UpdateEntity(
            MemberEntity existingEntity, Command command, MemberDataStore store)
        {
            var updated = MemberEntity.Create(command.Id, command.FirstName, command.LastName, command.Email);
            var index = store.Entities.FindIndex(y => y.Id == existingEntity.Id);
            store.Entities[index] = updated;

            await _db.Write();
            return MemberResponse.Map(updated);
        }
    }

    public static class Validator
    {
        public static Result<Command> Validate(Command command) =>
            ValidationErrors.Create()
                .AddIfError(() => command.Id <= 0, "Member.Id", "Member id must be valid and greater than 0.")
                .AddIfError(() => string.IsNullOrEmpty(command.FirstName), "Member.FirstName.Required", "Member first name is required.")
                .AddIfError(() => string.IsNullOrEmpty(command.LastName), "Member.Last.Required", "Member last name is required.")
                .AddIfError(() => string.IsNullOrEmpty(command.Email), "Member.Email.Required", "Member email is required.")
                .AddIfError(() => !EmailValidator.IsValidFormat(command.Email), "Member.Email.Invalid", "Member email is not the expected format (name@company.com).")
                .Map(() => command);
    }
}
