using MemberService.Common;

namespace MemberService.Endpoints.Members;

internal sealed class GetAllMembers
{
    public sealed record Command : ICommand<Result<MemberResponse[]>>;

    public sealed class Handler : ICommandHandlerAsync<Command, Result<MemberResponse[]>>
    {
        private readonly IMemberDb _db;

        public Handler(IMemberDb db) => _db = db;

        public async Task<Result<MemberResponse[]>> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            try
            {
                var store = await _db.Get();
                return store.Entities.Select(x => MemberResponse.Map(x)).ToArray();
            }
            catch (Exception ex)
            {
                return Result<MemberResponse[]>.Failure(ex);
            }
        }
    }
}
