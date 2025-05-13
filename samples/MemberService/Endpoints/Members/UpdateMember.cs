using MemberService.Common;

namespace MemberService.Endpoints.Members;

internal sealed class UpdateMember
{
    public sealed record Command(int Id, string FirstName, string LastName, string Email, string? CellPhone)
        : ICommand<Result<MemberResponse>>;

    public sealed class Handler : ICommandHandlerAsync<Command, Result<MemberResponse>>
    {
        private readonly IMemberDb _db;

        public Handler(IMemberDb db) => _db = db;

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
                .AddIfError(() => command.Id <= 0, Errors.IdInvalid)
                .AddIfError(() => string.IsNullOrEmpty(command.FirstName), Errors.FirstNameRequired)
                .AddIfError(() => string.IsNullOrEmpty(command.LastName), Errors.LastNameRequired)
                .AddIfError(() => string.IsNullOrEmpty(command.Email), Errors.EmailRequired)
                .AddIfError(() => !EmailValidator.IsValidFormat(command.Email), Errors.EmailInvalid)
                .Map(() => command);
    }
}
