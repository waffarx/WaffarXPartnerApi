using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferLookUp;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferSetting;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;

namespace WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
public interface IOfferSettingService
{
    Task<GenericResponse<List<OfferLookUpResponsetDto>>> GetOffersLookup();
    Task<GenericResponse<bool>> AddOrUpdateOfferLookUp(OfferLookUpRequestDto model);
    Task<GenericResponse<List<OfferResponseDto>>> GetOffers();
    Task<GenericResponse<bool>> AddOrUpdateOffer(OfferSettingRequestDto model);
    Task<GenericResponse<OfferDetailResponseDto>> GetOfferDetails(string id);
}
