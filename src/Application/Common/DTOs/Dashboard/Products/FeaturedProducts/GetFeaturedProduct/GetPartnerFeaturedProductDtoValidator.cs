using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts.GetFeaturedProduct;
public class GetPartnerFeaturedProductDtoValidator : AbstractValidator<GetPartnerFeaturedProductDto>
{
    public GetPartnerFeaturedProductDtoValidator()
    {
        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .WithMessage("Page number is required.")
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize)
            .NotEmpty()
            .WithMessage("Page size is required.")
            .LessThanOrEqualTo(10000)
            .WithMessage("Page size must be greater than 0.");
    }
}
