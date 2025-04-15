using MongoDB.Driver;
using WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.MongoRepository
{
    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task<IEnumerable<T>> GetAllByClientApiId(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("ClientApiId", id);

            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<T> GetByClientApiIdAsync(Guid id)
        {
            //var objectId = MongoDB.Bson.ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("ClientApiId", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, T entity)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

    }

    // Specific repositories for each collection
    //public class FeaturedProductSettingRepository : MongoRepository<FeaturedProductSetting>
    //{
    //    public FeaturedProductSettingRepository(IMongoCollection<FeaturedProductSetting> collection) 
    //        : base(collection)
    //    {
    //    }
        
    //    // Add custom query methods here
    //    public async Task<List<FeaturedProductSetting>> GetByClientApiIdAsync(int clientApiId)
    //    {
    //        var filter = Builders<FeaturedProductSetting>.Filter.Eq(x => x.ClientApiId, clientApiId);
    //        return await (await _collection.FindAsync(filter)).ToListAsync();
    //    }
    //}

    //// Similar repositories for other collections
    //public class ProductSearchSettingRepository : MongoRepository<ProductSearchSetting>
    //{
    //    public ProductSearchSettingRepository(IMongoCollection<ProductSearchSetting> collection) 
    //        : base(collection)
    //    {
    //    }
    //}

    //public class OfferSettingRepository : MongoRepository<OfferSetting>
    //{
    //    public OfferSettingRepository(IMongoCollection<OfferSetting> collection) 
    //        : base(collection)
    //    {
    //    }
    //}

    //public class StoreSearchSettingRepository : MongoRepository<StoreSearchSetting>
    //{
    //    public StoreSearchSettingRepository(IMongoCollection<StoreSearchSetting> collection) 
    //        : base(collection)
    //    {
    //    }
    //}
}
