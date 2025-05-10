using MemberService.Common;

namespace MemberService.Endpoints.Members;

internal sealed class GetMemberByEmail
{
    public sealed record Command(string Email) : ICommand<Result<MemberResponse>>;

    public sealed class Handler : ICommandHandlerAsync<Command, Result<MemberResponse>>
    {
        private readonly IMemberDb _db;

        public Handler(IMemberDb db) => _db = db;

        public async Task<Result<MemberResponse>> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            try
            {
                var store = await _db.Get();
                return Validator.Validate(command)
                                .Bind(c => store.GetEntityByEmail(command.Email))
                                .Map(entity => MemberResponse.Map(entity));
            }
            catch (Exception ex)
            {
                return Result<MemberResponse>.Failure(ex);
            }
        }
    }

    public static class Validator
    {
        public static Result<Command> Validate(Command command) =>
            ValidationErrors.Create()
                .AddIfError(() => string.IsNullOrEmpty(command.Email), Errors.EmailRequired)
                .AddIfError(() => !EmailValidator.IsValidFormat(command.Email), Errors.EmailInvalid)
                .Map(() => command);
    }
}