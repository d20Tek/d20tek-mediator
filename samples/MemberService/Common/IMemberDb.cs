namespace MemberService.Common;

internal interface IMemberDb
{
    Task<MemberDataStore> Get();

    Task Write();
}