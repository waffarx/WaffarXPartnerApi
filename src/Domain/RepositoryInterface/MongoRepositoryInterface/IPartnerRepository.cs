﻿using MongoDB.Bson;
using WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
using WaffarXPartnerApi.Domain.Entities.NoSqlEntities;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.FeaturedProducts;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;
public interface IPartnerRepository
{
    Task<PaginationResultModel<List<FeaturedProductModel>>> GetFeaturedProducts(int clientApiId, List<int> WhiteListIds, bool isActive = false, int pageNumber = 1, int pageSize = 20);
    Task<List<int>> GetWhiteListStores(int apiClientId, List<int> disabledStores);
    Task<OfferSetting> GetOfferSetting(int clientApiId, string offerSettingId);
    Task<bool> AddUpdateOfferSetting(OfferSettingModel model);
    Task<bool> DeleteFeaturedProductById(ObjectId productId, int clientApiId, int userId);
    Task<List<OfferSettingListingModel>> GetOfferSettingListing(int clientApiId, bool isEnglish = true);
    Task<List<StoreModel>> GetStoreDataByStoreIds(List<int> storeIds, bool isEnglish = true);
    Task<bool> AddOfferLookUp(OfferLookUpModel model);
    Task<List<OfferLookUpModel>> GetOfferLookUp(int clientApiId);
    Task<List<StoreLookUpModel>> GetAllStoreLookUp();
    Task<bool> UpdateWhiteListStore(UpdateWhiteListStoreRequestModel model);
    Task<List<int>> GetStoreIdsByStoreGuids(List<string> guids);
    Task<bool> UpdateFeaturedProduct(UpdateFeaturedProductModel model);
    Task<int> GetActiveFeaturedProductMaxRank(int clientApiId);
    Task<List<StoreIdsModel>> GetStoreIdsByGuids(List<string> guids);
    Task<bool> AddFeaturedProductList(List<AddFeaturedProductModel> products);
    Task<bool> SaveProductsRank(int clientApiId, int userId, List<FeaturedProductsBase> products);
    Task<List<DetailedStoreModel>> GetStoresDetails(List<int> storeIds, List<int> disabledStores = null);
    Task<OfferType> GetOfferType(int clientApiId, ObjectId offerTypeId);
    Task<bool> AddOfferType(OfferTypeModel model);
    Task<List<OfferTypeModel>> GetOfferTypes(int clientApiId);
}
