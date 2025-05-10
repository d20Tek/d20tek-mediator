using D20Tek.LowDb;
using MemberService.Common;

namespace MemberService.Persistence;

internal sealed class MemberDb : IMemberDb
{
    private readonly LowDbAsync<MemberDataStore> _lowDb;

    public MemberDb(LowDbAsync<MemberDataStore> lowDb) => _lowDb = lowDb;

    public Task<MemberDataStore> Get() => _lowDb.Get();

    public Task Write() => _lowDb.Write();
}
