using D20Tek.Functional;
using D20Tek.LowDb;
using MemberService.Endpoints.Members;

namespace MemberService.Persistence;

internal sealed class MemberFileRepository
{
    public static Failure<T> NotFoundError<T>(int id) where T : notnull =>
        Error.NotFound("MemberEntity.NotFound", $"Entity with id={id} not found.");

    public static Failure<int> AlreadyExistsError(int id) =>
        Error.Conflict("MemberEntity.AlreadyExists", $"Entity with id={id} already exists.");

    private readonly LowDb<MemberDataStore> _db;

    public MemberFileRepository(LowDb<MemberDataStore> db) => _db = db;

    public MemberEntity[] GetEntities() => _db.Get().Entities.ToArray() ?? [];

    public Result<MemberEntity> GetEntityById(int id) =>
        _db.Get().Entities
                 .Where(x => x.Id == id)
                 .Select(entity => (Result<MemberEntity>)entity)
                 .DefaultIfEmpty(NotFoundError<MemberEntity>(id))
                 .First();

    public Result<MemberEntity> Create(MemberEntity entity) =>
        _db.Get().ToIdentity()
                 .Map(store => EnsureUniqueId(store)
                    .Map(id =>
                    {
                        entity.SetId(id);
                        return Save(entity, () => store.Entities.Add(entity));
                    }));

    public Result<MemberEntity> Delete(int id) =>
        GetEntityById(id)
            .Map(e => Save(e, () => _db.Get().Entities.RemoveAll(y => y.Id == id)));

    public Result<MemberEntity[]> DeleteMany(MemberEntity[] entities) =>
        Result<IEnumerable<int>>.Success(entities.Select(e => e.Id))
            .Map(ids =>
                _db.Get().ToIdentity()
                    .Iter(store => ids.ForEach(id => store.Entities.RemoveAll(x => x.Id == id)))
                    .Iter(_ => _db.Write()))
                    .Map(_ => entities);

    public Result<MemberEntity> Update(MemberEntity entity) =>
        GetEntityById(entity.Id)
            .Map(_ => _db.Get())
            .Map(store => Save(entity, () => store.Entities[GetEntityIndex(store, entity)] = entity));

    private MemberEntity Save(Identity<MemberEntity> entry, Action op) =>
        entry.Iter(_ => op())
             .Iter(e => _db.Write());

    private static Result<int> EnsureUniqueId(MemberDataStore element) =>
        element.GetNextId().ToIdentity()
            .Map(newId => (element.Entities.Any(x => x.Id == newId)) ? AlreadyExistsError(newId) : Result<int>.Success(newId));

    private static int GetEntityIndex(MemberDataStore store, MemberEntity entity) =>
        store.Entities.FindIndex(y => y.Id == entity.Id);
}
