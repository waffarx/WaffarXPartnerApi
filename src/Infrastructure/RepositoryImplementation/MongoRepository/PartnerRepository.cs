using MongoDB.Bson;
using MongoDB.Driver;
using WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
using System.Linq.Expressions;
using WaffarXPartnerApi.Domain.Entities.NoSqlEntities;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.FeaturedProducts;
using MongoDB.Bson.Serialization;
using System.Linq;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.MongoRepository;
public class PartnerRepository : IPartnerRepository
{
    private readonly IMongoCollection<FeaturedProductSetting> _featuredProductCollection;
    private readonly IMongoCollection<StoreSearchSetting> _storeSearchSettingsCollection;
    private readonly IMongoCollection<FeaturedProductSettingAudit> _featuredProductSettingAuditCollection;
    private readonly IMongoCollection<OfferSetting> _offerSettingsCollection;
    private readonly IMongoCollection<OfferLookUp> _offerLookupsCollection;
    private readonly IMongoCollection<StoreLookUp> _storeLookUpCollection;
    private readonly IMongoCollection<OfferType> _offertypesCollection;
    private readonly IMongoCollection<StoreSearchSettingAudit> _storeSearchSettingAuditCollection;
    public PartnerRepository(IMongoDatabase database, IMongoCollection<OfferLookUp> offerLookupsCollection,
        IMongoCollection<StoreLookUp> storeLookUpCollection, 
        IMongoCollection<StoreSearchSettingAudit> storeSearchSettingAuditCollection)
    {
        _featuredProductCollection = database.GetCollection<FeaturedProductSetting>("FeaturedProductSetting");
        _storeSearchSettingsCollection = database.GetCollection<StoreSearchSetting>("StoreSearchSetting");
        _featuredProductSettingAuditCollection = database.GetCollection<FeaturedProductSettingAudit>("FeaturedProductSettingAudit");
        _offerSettingsCollection = database.GetCollection<OfferSetting>("OfferSetting");
        _offertypesCollection = database.GetCollection<OfferType>("OfferTypes");
        _offerLookupsCollection = offerLookupsCollection;
        _storeLookUpCollection = storeLookUpCollection;
        _storeSearchSettingAuditCollection = storeSearchSettingAuditCollection;
    }

    public async Task<bool> AddUpdateOfferSetting(OfferSettingModel model)
    {
        try
        {
            if (model.Id == null)
            {
                await _offerSettingsCollection.InsertOneAsync(new OfferSetting
                {
                    ClientApiId = model.ClientApiId,
                    OfferLookUpId = new ObjectId(model.OfferLookUpId),
                    OfferTypeId = new ObjectId(model.OfferTypeId),
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsProductLevel = model.IsProductLevel,
                    IsStoreLevel = model.IsStoreLevel,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = model.UserId,
                    StoreIds = model.IsStoreLevel ? model.StoreIds : new List<int>(),
                    ProductIds = model.IsProductLevel ? model.ProductIds?.Select(id => new ObjectId(id)).ToList() : new List<ObjectId>(),
                });
            }
            else
            {
                var filter = Builders<OfferSetting>.Filter.Eq(s => s.ClientApiId, model.ClientApiId) &
               Builders<OfferSetting>.Filter.Eq(s => s.Id, new ObjectId(model.Id));

                // Create a base update definition
                var updateDefinition = Builders<OfferSetting>.Update
                    .Set(s => s.OfferLookUpId, new ObjectId(model.OfferLookUpId))
                    .Set(s => s.OfferTypeId, new ObjectId(model.OfferTypeId))
                    .Set(s => s.StartDate, model.StartDate)
                    .Set(s => s.EndDate, model.EndDate)
                    .Set(s => s.IsProductLevel, model.IsProductLevel)
                    .Set(s => s.IsStoreLevel, model.IsStoreLevel)
                    .Set(s => s.UpdatedBy, model.UserId);

                // Handle product level update
                if (model.IsProductLevel)
                {
                    updateDefinition = updateDefinition
                        .Set(s => s.ProductIds, model.ProductIds?.Select(id => new ObjectId(id)).ToList())
                        .Set(s => s.StoreIds, new List<int>());
                }
                // Handle store level update
                else if (model.IsStoreLevel)
                {
                    updateDefinition = updateDefinition
                        .Set(s => s.StoreIds, model.StoreIds)
                        .Set(s => s.ProductIds, new List<ObjectId>());
                }

                // Execute the update with the complete update definition
                await _offerSettingsCollection.UpdateOneAsync(filter, updateDefinition);
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PaginationResultModel<List<FeaturedProductModel>>> GetFeaturedProducts(int clientApiId, List<int> WhiteListIds, bool isActive = false, int pageNumber = 1, int pageSize = 20)
    {
        PaginationResultModel<List<FeaturedProductModel>> paginatedList = new PaginationResultModel<List<FeaturedProductModel>>();
        try
        {
            // Get total count
            var totalCount = await _featuredProductCollection.CountDocumentsAsync(x => x.ClientApiId == clientApiId && WhiteListIds.Contains(x.Product.StoreId));
            if (totalCount > 0)
            {
                // Build sort 
                var sortBuilder = Builders<FeaturedProductSetting>.Sort
                    .Descending(x => x.Product.EndDate)
                    .Ascending(x => x.Product.ProductRank);
                // Build filter 
                Expression<Func<FeaturedProductSetting, bool>> filter = p => p.ClientApiId == clientApiId && WhiteListIds.Contains(p.Product.StoreId);
                if (isActive == true)
                {
                    // Get the current date
                    DateTime nowDate = DateTime.Now;

                    filter = p => p.ClientApiId == clientApiId && p.Product.EndDate >= nowDate;
                    sortBuilder = Builders<FeaturedProductSetting>.Sort.Ascending(x => x.Product.ProductRank);
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

    public async Task<OfferSetting> GetOfferSetting(int clientApiId, string offerSettingId)
    {
        try
        {
            var filter = Builders<OfferSetting>.Filter.Eq(s => s.ClientApiId, clientApiId) &
                         Builders<OfferSetting>.Filter.Eq(s => s.Id, new ObjectId(offerSettingId));
            return await _offerSettingsCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<OfferType> GetOfferType(int clientApiId, ObjectId offerTypeId)
    {
        try
        {
            var filter = Builders<OfferType>.Filter.Eq(s => s.ClientApiId, clientApiId) &
                         Builders<OfferType>.Filter.Eq(s => s.Id, offerTypeId);
            return await _offertypesCollection.Find(filter).FirstOrDefaultAsync();
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
                    Builders<FeaturedProductSetting>.Filter.Eq(x => x.ClientApiId, clientApiId),
                    Builders<FeaturedProductSetting>.Filter.Eq(x => x.Product.ProductId, productId)
            );
            var featuredProduct = await _featuredProductCollection.Find(filter).FirstOrDefaultAsync();
            // Delete the featured product
            var result = await _featuredProductCollection.DeleteOneAsync(filter);
            if (result.DeletedCount > 0 && featuredProduct != null)
            {
                await _featuredProductSettingAuditCollection.InsertOneAsync(new FeaturedProductSettingAudit
                {
                    ClientApiId = clientApiId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    OriginalDocument = featuredProduct,
                    Type = "Delete",
                });
            }
            return result.DeletedCount > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<OfferSettingListingModel>> GetOfferSettingListing(int clientApiId, bool isEnglish = true)
    {
        try
        {
            // Filter OfferSettings by ClientApiId  
            var filter = Builders<OfferSetting>.Filter.Eq(s => s.ClientApiId, clientApiId);
            var offerSettings = await _offerSettingsCollection.Find(filter).ToListAsync();
            if (offerSettings == null || !offerSettings.Any())
            {
                return new List<OfferSettingListingModel>();
            }

            var offerLookupIds = offerSettings.Select(x => x.OfferLookUpId).Distinct().ToList();
            var offerLookups = await _offerLookupsCollection.Find(l => offerLookupIds.Contains(l.Id)).ToListAsync();

            var offerTypeIds = offerSettings.Select(x => x.OfferTypeId).Distinct().ToList();
            var offerTypes = await _offertypesCollection.Find(l => offerTypeIds.Contains(l.Id)).ToListAsync();

            var result = new List<OfferSettingListingModel>();

            foreach (var offerSetting in offerSettings)
            {
                // Fetch the associated OfferLookUp  
                var offerLookup = offerLookups.Where(l => l.Id == offerSetting.OfferLookUpId).FirstOrDefault();
                var offerType = offerTypes.Where(l => l.Id == offerSetting.OfferTypeId).FirstOrDefault();

                result.Add(new OfferSettingListingModel
                {
                    OfferId = offerSetting.Id.ToString(), // Convert ObjectId to Guid  
                    OfferName = isEnglish ? offerLookup?.NameEn : offerLookup?.NameAr,
                    StartDate = offerSetting.StartDate,
                    EndDate = offerSetting.EndDate,
                    IsProductLevel = offerSetting.IsProductLevel,
                    IsStoreLevel = offerSetting.IsStoreLevel,
                    OfferTypeId = offerSetting.OfferTypeId.ToString(),
                });
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<StoreModel>> GetStoreDataByStoreIds(List<int> storeIds, bool isEnglish = true)
    {
        try
        {
            var storeFilter = Builders<StoreLookUp>.Filter.In(s => s.StoreId, storeIds);
            var storeLookUps = await _storeLookUpCollection.Find(storeFilter).ToListAsync();
            if (storeLookUps == null || !storeLookUps.Any())
            {
                return new List<StoreModel>();
            }
            var response = new List<StoreModel>();
            if (storeLookUps != null && storeLookUps.Any())
            {
                foreach (var store in storeLookUps)
                {
                    response.Add(new StoreModel
                    {
                        Id = store.StoreGuid,
                        Name = isEnglish ? store.NameEn : store.NameAr,
                        Logo = store.LogoUrl,
                        LogoPng = store.LogoPngUrl
                    });
                }
            }
            return response;
        }
        catch (Exception)
        {
            throw;
        }


    }

    public async Task<bool> AddOfferLookUp(OfferLookUpModel model)
    {
        try
        {
            if (model.Id == null)
            {
                await _offerLookupsCollection.InsertOneAsync(new OfferLookUp
                {
                    ClientApiId = model.ClientApiId,
                    NameAr = model.NameAr,
                    NameEn = model.NameEn,
                    DescriptionEn = model.DescriptionEn,
                    DescriptionAr = model.DescriptionAr,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = model.UserId,
                    UniqueKey = model.NameEn.ToUpper() + model.NameAr.ToUpper(),
                });
            }
            else
            {
                var filter = Builders<OfferLookUp>.Filter.Eq(s => s.ClientApiId, model.ClientApiId) &
                             Builders<OfferLookUp>.Filter.Eq(s => s.Id, new ObjectId(model.Id));

                var update = Builders<OfferLookUp>.Update
                    .Set(s => s.NameAr, model.NameAr)
                    .Set(s => s.NameEn, model.NameEn)
                    .Set(s => s.DescriptionAr, model.DescriptionAr)
                    .Set(s => s.DescriptionEn, model.DescriptionEn)
                    .Set(s => s.UpdatedBy, model.UserId);
                await _offerLookupsCollection.UpdateOneAsync(filter, update);
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<OfferLookUpModel>> GetOfferLookUp(int clientApiId)
    {
        try
        {
            var filter = Builders<OfferLookUp>.Filter.Eq(s => s.ClientApiId, clientApiId);
            var offerLookUp = await _offerLookupsCollection.Find(filter).ToListAsync();
            if (offerLookUp == null || !offerLookUp.Any())
            {
                return new List<OfferLookUpModel>();
            }
            var response = new List<OfferLookUpModel>();
            foreach (var offer in offerLookUp)
            {
                response.Add(new OfferLookUpModel
                {
                    Id = offer.Id.ToString(),
                    NameEn = offer.NameEn,
                    NameAr = offer.NameAr,
                    DescriptionEn = offer.DescriptionEn,   
                    DescriptionAr = offer.DescriptionAr,
                });
            }
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<StoreLookUpModel>> GetAllStoreLookUp()
    {
        try
        {
            var filter = Builders<StoreLookUp>.Filter.Eq(s => s.IsActive, true);

            var allStores = await _storeLookUpCollection.Find(filter).ToListAsync();
            if (allStores == null || !allStores.Any())
            {
                return new List<StoreLookUpModel>();
            }
            var response = new List<StoreLookUpModel>();
            foreach (var store in allStores)
            {
                response.Add(new StoreLookUpModel
                {
                    Id = store.StoreGuid,
                    StoreId = store.StoreId,
                    NameEn = store.NameEn,
                    NameAr = store.NameAr,
                    Logo = store.LogoUrl,
                    LogoPng = store.LogoPngUrl
                });
            }
            return response;
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdateWhiteListStore(UpdateWhiteListStoreRequestModel model)
    {
        try
        {
            // Find the existing store setting by ClientId
            var filter = Builders<StoreSearchSetting>.Filter.Eq(s => s.ClientApiId, model.ClientApiId);
            var existingStoreSetting = await _storeSearchSettingsCollection.Find(filter).FirstOrDefaultAsync();

            var storesToAddIds = model.StoreList.Select(s => s.StoreId).ToList();
            var storesToAddGuids = storesToAddIds
                       .Select(s => Guid.TryParse(s, out var guid) ? guid : (Guid?)null)
                       .Where(guid => guid != null)
                       .Select(guid => guid.Value)
                       .ToList();
            var storeLookup = await _storeLookUpCollection.Find(s => storesToAddGuids.Contains(s.StoreGuid)).ToListAsync();
            List<StoreSettings> storeSettings = new List<StoreSettings>();
            foreach (var store in model.StoreList)
            {
                storeSettings.Add(new StoreSettings
                {
                    IsFeatured = store.IsFeatured,
                    Rank = store.Rank,
                    StoreId = storeLookup.Where(x => x.StoreGuid == Guid.Parse(store.StoreId)).FirstOrDefault()?.StoreId ?? 0,
                    BackgroundColor = store.BackgroundColor,
                });
            }

            if (existingStoreSetting != null)
            {
                // Add an audit log for the store setting update
                var auditLog = new StoreSearchSettingAudit
                {
                    ClientApiId = model.ClientApiId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = model.UserId,
                    OriginalDocument = existingStoreSetting,
                };

                await _storeSearchSettingAuditCollection.InsertOneAsync(auditLog);

                // Update the existing store setting
                var update = Builders<StoreSearchSetting>.Update
                    .Set(s => s.Stores, storeSettings)
                    .Set(s => s.UpdatedAt, DateTime.UtcNow)
                    .Set(s => s.UpdatedBy, model.UserId);

                await _storeSearchSettingsCollection.UpdateOneAsync(filter, update);
            }
            else
            {
                // Insert a new store setting if it doesn't exist
                var newStoreSetting = new StoreSearchSetting
                {
                    ClientApiId = model.ClientApiId,
                    SettingType = "whitelisedStores",
                    Stores = storeSettings,
                    CreatedBy = model.UserId,
                    CreatedAt = DateTime.UtcNow,

                };

                await _storeSearchSettingsCollection.InsertOneAsync(newStoreSetting);
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<int>> GetStoreIdsByStoreGuids(List<string> guids)
    {
        try
        { 
            var storeGuids = guids.Select(g => Guid.TryParse(g, out var guid) ? guid : (Guid?)null)
                .Where(guid => guid != null)
                .Select(guid => guid.Value)
                .ToList();

            var storeLookup = await _storeLookUpCollection.Find(s => storeGuids.Contains(s.StoreGuid)).ToListAsync();
            return storeLookup.Select(x => x.StoreId).ToList();

        }
        catch(Exception)
        {
            throw;
        }
    }
    public async Task<List<StoreIdsModel>> GetStoreIdsByGuids(List<string> guids)
    {
        try
        {
            var storeGuids = guids.Select(g => Guid.TryParse(g, out var guid) ? guid : (Guid?)null)
            .Where(guid => guid != null)
            .Select(guid => guid.Value)
            .ToList();
            if (storeGuids?.Count > 0)
            {
                var storeLookup = await _storeLookUpCollection.Find(s => storeGuids.Contains(s.StoreGuid)).ToListAsync();
                return storeLookup.Select(x => new StoreIdsModel
                {
                    StoreId = x.StoreId,
                    StoreGuid = x.StoreGuid
                }).ToList();
            }
            return new List<StoreIdsModel>();   
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> UpdateFeaturedProduct(UpdateFeaturedProductModel model)
    {
        try
        {
            var filter = Builders<FeaturedProductSetting>.Filter.And(
                    Builders<FeaturedProductSetting>.Filter.Eq(x => x.ClientApiId, model.ClientApiId),
                    Builders<FeaturedProductSetting>.Filter.Eq(x => x.Product.ProductId, new ObjectId(model.ProductId))
            );
            var featuredProduct = await _featuredProductCollection.Find(filter).FirstOrDefaultAsync();
            // Check if existed the featured product
            
            if (featuredProduct != null)
            {
                // Create a base update definition
                var updateDefinition = Builders<FeaturedProductSetting>.Update
                    .Set(s => s.Product.StartDate, model.StartDate)
                    .Set(s => s.Product.EndDate, model.EndDate)
                    .Set(s => s.UpdatedBy, model.UserId)
                    .Set(s => s.UpdatedAt, DateTime.UtcNow);

                var result = await _featuredProductCollection.UpdateOneAsync(filter, updateDefinition);

                bool isUpdated = result.ModifiedCount > 0;
                if (isUpdated)
                {
                    await _featuredProductSettingAuditCollection.InsertOneAsync(new FeaturedProductSettingAudit
                    {
                        ClientApiId = model.ClientApiId,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = model.UserId,
                        OriginalDocument = featuredProduct,
                        Type = "Update",
                    });
                }
                return isUpdated;
            }
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<int> GetActiveFeaturedProductMaxRank(int clientApiId)
    {
        try
        {
            var filter = Builders<FeaturedProductSetting>.Filter.And(
                    Builders<FeaturedProductSetting>.Filter.Eq(x => x.ClientApiId, clientApiId),
                    Builders<FeaturedProductSetting>.Filter.Gte(x => x.Product.EndDate, DateTime.Now)
            );
            var featuredProduct = await _featuredProductCollection.Find(filter).SortByDescending(x => x.Product.ProductRank).FirstOrDefaultAsync();
            if (featuredProduct != null)
            {
                return featuredProduct.Product.ProductRank;
            }
            return 0;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> AddFeaturedProductList(List<AddFeaturedProductModel> products)
    {
        try
        {
            var featuredProducts = products.Select(x => new FeaturedProductSetting
            {
                ClientApiId = x.ClientApiId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = x.UserId,
                Product = new ProductFeature
                {
                    StoreId = x.StoreIdInt,
                    ProductId = x.ProductId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ProductRank = x.ProductRank
                },
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = x.UserId,   
                SettingType = "FeaturedProduct"
            }).ToList();

            await _featuredProductCollection.InsertManyAsync(featuredProducts);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> SaveProductsRank(int clientApiId, int userId, List<FeaturedProductsBase> products)
    {
        try
        {
            var productIds = products.Select(x => x.ProductId).ToList();
            var allfilter = Builders<FeaturedProductSetting>.Filter.And(
                Builders<FeaturedProductSetting>.Filter.Eq(x => x.ClientApiId, clientApiId),
                Builders<FeaturedProductSetting>.Filter.In(x => x.Product.ProductId, productIds)
            );
            var featuredProducts = await _featuredProductCollection.Find(allfilter).ToListAsync();

            var updates = new List<WriteModel<FeaturedProductSetting>>();
            foreach (var product in products)
            {
                var filter = Builders<FeaturedProductSetting>.Filter.And(
                    Builders<FeaturedProductSetting>.Filter.Eq(x => x.ClientApiId, clientApiId),
                    Builders<FeaturedProductSetting>.Filter.Eq(x => x.Product.ProductId, product.ProductId)
                );

                var update = Builders<FeaturedProductSetting>.Update
                    .Set(x => x.Product.ProductRank, product.ProductRank)
                    .Set(x => x.UpdatedAt, DateTime.UtcNow)
                    .Set(x => x.UpdatedBy, product.UserId);

                updates.Add(new UpdateOneModel<FeaturedProductSetting>(filter, update));
            }
            if (updates.Count == 0)
                return false;

            var result = await _featuredProductCollection.BulkWriteAsync(updates);
            if (result.ModifiedCount > 0)
            {
                var updatedFeaturedProducts = featuredProducts.Select(x => new FeaturedProductSettingAudit
                {
                    ClientApiId = x.ClientApiId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    Type = "Update",
                    OriginalDocument = x,
                }).ToList();
                await _featuredProductSettingAuditCollection.InsertManyAsync(updatedFeaturedProducts);
            }
            return result.ModifiedCount > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<DetailedStoreModel>> GetStoresDetails(List<int> storeIds, List<int> disabledStores = null)
    {
        List<DetailedStoreModel> storeList = new List<DetailedStoreModel>();
        try
        {
            // Adjust the filter to use the correct type for the collection  
            var filter = Builders<StoreSearchSetting>.Filter.In("Stores.StoreId", storeIds);
            if (disabledStores?.Count > 0)
            {
                filter = Builders<StoreSearchSetting>.Filter.And(
                    Builders<StoreSearchSetting>.Filter.In("Stores.StoreId", storeIds),
                    Builders<StoreSearchSetting>.Filter.Nin("Stores.StoreId", disabledStores)
                );
            }

            // Adjust the projection to match the type of the collection  
            var projection = Builders<StoreSearchSetting>.Projection
                .Exclude("_id");

            var result = await _storeSearchSettingsCollection.Find(filter).Project<StoreSearchSetting>(projection).ToListAsync();
            if (result?.Count > 0)
            {
                foreach (var doc in result)
                {
                    storeList.Add(new DetailedStoreModel
                    {
                        // Map the properties from StoreSearchSetting to DetailedStoreModel  
                        // Adjust the mapping logic as per your data structure  
                    });
                }
            }
            return storeList;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> AddOfferType(OfferTypeModel model)
    {
        try
        {
            if (model.Id == null)
            {
                await _offertypesCollection.InsertOneAsync(new OfferType
                {
                    ClientApiId = model.ClientApiId,
                    NameAr = model.NameAr,
                    NameEn = model.NameEn,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = model.UserId,
                });
            }
            else
            {
                var filter = Builders<OfferType>.Filter.Eq(s => s.ClientApiId, model.ClientApiId) &
                             Builders<OfferType>.Filter.Eq(s => s.Id, new ObjectId(model.Id));

                var update = Builders<OfferType>.Update
                    .Set(s => s.NameAr, model.NameAr)
                    .Set(s => s.NameEn, model.NameEn)
                    .Set(s => s.UpdatedBy, model.UserId);
                await _offertypesCollection.UpdateOneAsync(filter, update);
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<OfferTypeModel>> GetOfferTypes(int clientApiId)
    {
        try
        {
            var filter = Builders<OfferType>.Filter.Eq(s => s.ClientApiId, clientApiId);
            var offerTypes = await _offertypesCollection.Find(filter).ToListAsync();
            if (offerTypes == null || !offerTypes.Any())
            {
                return new List<OfferTypeModel>();
            }
            var response = new List<OfferTypeModel>();
            foreach (var type in offerTypes)
            {
                response.Add(new OfferTypeModel
                {
                    Id = type.Id.ToString(),
                    NameEn = type.NameEn,
                    NameAr = type.NameAr
                });
            }
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
