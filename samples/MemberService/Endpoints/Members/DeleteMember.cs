using MemberService.Common;

namespace MemberService.Endpoints.Members;

internal sealed class DeleteMember
{
    public sealed record Command(int Id) : ICommand<Result<MemberResponse>>;

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
                                      .MapAsync(entity => RemoveEntity(entity, store));
            }
            catch (Exception ex)
            {
                return Result<MemberResponse>.Failure(ex);
            }
        }

        private async Task<MemberResponse> RemoveEntity(MemberEntity entity, MemberDataStore store)
        {
            store.Entities.RemoveAll(y => y.Id == entity.Id);
            await _db.Write();
            return MemberResponse.Map(entity);
        }
    }

    public static class Validator
    {
        public static Result<Command> Validate(Command command) =>
            ValidationErrors.Create()
                .AddIfError(() => command.Id <= 0, Errors.IdInvalid)
                .Map(() => command);
    }
}
