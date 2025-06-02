using WaffarXPartnerApi.Application.Common.DTOs.Shared.ExitClick;

namespace WaffarXPartnerApi.Application.Common.DTOs.Helper;
public static class RequestDataHelper
{
    public static CreateExitClickRequestDto CreateExitClickRequestData(int userId,int callSource, bool isEnglish , string section, Guid storeId, string productId = "", string userIdentifier = ""
        , string shoppingTripIdentifier = "", string variant = "", int clientApiId = 0)
    {
        return new CreateExitClickRequestDto
        {
            UserId = userId,
            ClientApiId = clientApiId,
            CallSource = callSource,
            Section = section,
            AdvertiserGuid = storeId,
            ProductSearchId = productId,     
            ClickSource = 8,
            IsEnglish = isEnglish,
            ProductId = 0,
            CountryId = 67,
            IsMobile = false,
            LinkId = 0,
            MobileType = "",
            TrackParamKeys = new Dictionary<string, object>() ,
            XClid = "", 
            partnerUserIdentifier = userIdentifier,
            subId1 = shoppingTripIdentifier,   
            subId2 = "",
            variant = variant
        };
    }
}
