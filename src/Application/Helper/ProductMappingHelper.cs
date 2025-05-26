using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Domain.Models.SharedModels;

namespace WaffarXPartnerApi.Application.Helper;
public static class ProductMappingHelper
{
    public static BaseProductSearchResultDto MapToBaseProduct(ProductSearchResponseModel model)
    {
        try
        {
            if (model == null)
                return null;
            BaseProductSearchResultDto res = new BaseProductSearchResultDto
            {
                Id = model.Id,
                Name = model.Name,
                MerchantName = model.AdvertiserName,
                Price = model.Price,
                Brand = model.Brand,
                Currency = model.Currency,
                PrimaryImg = model.PrimaryImg,
                VariantsImgs = model.VariantsImgs,
                Category = model.Category,
                ErrorImg = model.ErrorImg,
                OldPrice = model.OldPrice,
                OldPriceText = model.OldPriceText,
                PriceText = model.PriceText,
                Store = model.Store == null ? new StoreResponseDto() :
                new StoreResponseDto
                {
                    Id = (Guid)(model.Store?.Id),
                    Logo = model?.Store?.Logo,
                    Name = model?.Store?.Name,
                    LogoPng = model?.Store?.LogoPng,
                    ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + model.Store.Id,
                    ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + model.Store.Id,
                },
                Offers = model?.Offers,
                ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id,
                ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id
            };
            return res;

        }
        catch (Exception)
        {
            throw;
        }

    }
    public static DetailedProductSearchResultDto MapToDetailProduct(ProductSearchResponseModel model)
    {
        try
        {
            if (model == null)
                return null;
            DetailedProductSearchResultDto res = new DetailedProductSearchResultDto
            {
                Id = model.Id,
                Name = model.Name,
                MerchantName = model.AdvertiserName,
                Price = model.Price,
                Brand = model.Brand,
                Currency = model.Currency,
                PrimaryImg = model.PrimaryImg,
                VariantsImgs = model.VariantsImgs,
                Category = model.Category,
                ErrorImg = model.ErrorImg,
                OldPrice = model.OldPrice,
                OldPriceText = model.OldPriceText,
                PriceText = model.PriceText,
                Store = model.Store == null ? new StoreResponseDto() :
                new StoreResponseDto
                {
                    Id = (Guid)(model.Store?.Id),
                    Logo = model?.Store?.Logo,
                    Name = model?.Store?.Name,
                    LogoPng = model?.Store?.LogoPng,
                    ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + model.Store.Id,
                    ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + model.Store.Id

                },
                Offers = model?.Offers,
                Description = model.Description,
                DiscountedText = model.DiscountedText,
                Discounted = model.Discounted,
                Feature = model.Features,
                SKU = model.SKU,
                Warranty = model.Warranty,
                Specification = model.Specification?.ToDictionary(k => k.Key, v => v.Value),
                Options = (model.options as IEnumerable<OptionDto>)?.Select(o => new OptionDto
                {
                    Name = o.Name,
                    Id = o.Id,
                    ProductId = o.ProductId,
                    Position = o.Position,
                    Values = o.Values
                }).ToList(),
                PriceVariants = model.price_variants?.Select(v => new PriceVariantDto
                {
                    Price = v.price,
                    Title = v.title,
                    Available = v.available,
                    Options = v.options,
                    PriceText = v.PriceText,
                    DiscountedText = v.DiscountedText,
                    Discounted = v.Discounted,
                    OldPrice = v.old_price,
                    OldPriceText = v.OldPriceText,
                    ShoppingURL = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id + StaticValues.Variant + v.variant_id,
                    ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id
                }).ToList(),
                ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id,
                ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id

            };
            return res;

        }
        catch (Exception)
        {
            throw;
        }
    }
}
