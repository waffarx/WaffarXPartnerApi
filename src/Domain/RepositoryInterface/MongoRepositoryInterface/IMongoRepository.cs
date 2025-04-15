namespace WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;
public interface IMongoRepository<T>
{
    Task<IEnumerable<T>> GetAllByClientApiId(Guid id);
    Task<T> GetByClientApiIdAsync(Guid id);
    Task CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
}
