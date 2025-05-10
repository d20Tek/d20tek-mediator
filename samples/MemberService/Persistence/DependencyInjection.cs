using D20Tek.LowDb;
using MemberService.Common;

namespace MemberService.Persistence;

internal static class DependencyInjection
{
    public const string _databaseFile = "member-data.json";

    public static IServiceCollection AddPersistence(this IServiceCollection services) =>
        services.AddLowDbAsync<MemberDataStore>(_databaseFile)
                .AddSingleton<IMemberDb, MemberDb>();
}
