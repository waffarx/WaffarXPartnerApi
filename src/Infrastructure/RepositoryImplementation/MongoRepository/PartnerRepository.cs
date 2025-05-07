using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
using System.Linq.Expressions;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.MongoRepository;
public class PartnerRepository : IPartnerRepository
{
    private readonly IMongoCollection<FeaturedProductSetting> _featuredProductCollection;
    private readonly IMongoCollection<StoreSearchSetting> _storeSearchSettingsCollection;
    private readonly IMongoCollection<FeaturedProductSettingAudit> _featuredProductSettingAuditCollection;
    public PartnerRepository(IMongoDatabase database)
    {
        _featuredProductCollection = database.GetCollection<FeaturedProductSetting>("FeaturedProductSetting");
        _storeSearchSettingsCollection = database.GetCollection<StoreSearchSetting>("StoreSearchSetting");
        _featuredProductSettingAuditCollection = database.GetCollection<FeaturedProductSettingAudit>("FeaturedProductSettingAudit");
    }

    public async Task<PaginationResultModel<List<FeaturedProductModel>>> GetFeaturedProducts(int clientApiId, List<int> WhiteListIds ,bool isActive = false ,int pageNumber = 1, int pageSize = 20)
    {
        PaginationResultModel<List<FeaturedProductModel>> paginatedList = new PaginationResultModel<List<FeaturedProductModel>>();
        try
        {
            // Get total count
            var totalCount = await _featuredProductCollection.CountDocumentsAsync(x => x.ClientApiId == clientApiId && WhiteListIds.Contains(x.Product.StoreId));
            if (totalCount > 0)
            {
                // Build sort 
                var sortBuilder = Builders<FeaturedProductSetting>.Sort.Ascending(x => x.Product.ProductRank);
                // Build filter 
                Expression<Func<FeaturedProductSetting, bool>> filter = p => p.ClientApiId == clientApiId && WhiteListIds.Contains(p.Product.StoreId);
                if (isActive == true)
                {
                    // Get the current date
                    DateTime nowDate = DateTime.Now;

                    filter = p => p.ClientApiId == clientApiId 
                    && (p.Product.StartDate <= nowDate && p.Product.EndDate >= nowDate);
                }
                // Get paged data
                var featuredList = await _featuredProductCollection.Find(filter)
                    .Sort(sortBuilder)
                    .Project(p => new FeaturedProductModel
                    {
                        StoreId = p.Product.StoreId,
                        ProductId = p.Product.ProductId,
                        ProductRank = p.Product.ProductRank,
                        StartDate = p.Product.StartDate,
                        EndDate = p.Product.EndDate
                    })
                    .Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

                if (featuredList?.Count > 0)
                {
                    paginatedList.Data = featuredList;
                    paginatedList.TotalRecords = (int)totalCount;
                }
            }
            return paginatedList;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<List<int>> GetWhiteListStores(int apiClientId, List<int> disabledStores)
    {
        try
        {
            var filter = Builders<StoreSearchSetting>.Filter.And(
                    Builders<StoreSearchSetting>.Filter.Eq("ClientApiId", apiClientId),
                    Builders<StoreSearchSetting>.Filter.Nin("StoreId", disabledStores)
                );
            var result = await _storeSearchSettingsCollection.Find(filter).FirstOrDefaultAsync();

            if (result != null && result.Stores?.Count > 0)
            {
                return result.Stores.Select(x => x.StoreId).ToList();
            }

            return new List<int>();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> DeleteFeaturedProductById(ObjectId productId, int clientApiId, int userId)
    {
        try
        {
            var filter = Builders<FeaturedProductSetting>.Filter.And(
                    Builders<FeaturedProductSetting>.Filter.Eq(x=> x.ClientApiId, clientApiId),
                    Builders<FeaturedProductSetting>.Filter.Eq(x => x.Product.ProductId, productId)
            );

            // Delete the featured product
            var result = await _featuredProductCollection.DeleteOneAsync(filter);
            if (result.DeletedCount > 0)
            {
                var featuredProduct = await _featuredProductCollection.Find(filter).FirstOrDefaultAsync();
                await _featuredProductSettingAuditCollection.InsertOneAsync(new FeaturedProductSettingAudit
                {
                    ClientApiId = clientApiId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    OriginalDocument = featuredProduct
                });
            }
            return result.DeletedCount > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

}
