using D20Tek.Functional;
using D20Tek.LowDb;
using D20Tek.Mediator;
using MemberService.Persistence;

namespace MemberService.Endpoints.Members;

internal sealed class GetAllMembers
{
    public class Command : ICommand<Result<MemberResponse[]>>;

    public class Handler : ICommandHandlerAsync<Command, Result<MemberResponse[]>>
    {
        private readonly LowDbAsync<MemberDataStore> _db;

        public Handler(LowDbAsync<MemberDataStore> db) => _db = db;

        public async Task<Result<MemberResponse[]>> HandleAsync(Command request, CancellationToken cancellationToken)
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
