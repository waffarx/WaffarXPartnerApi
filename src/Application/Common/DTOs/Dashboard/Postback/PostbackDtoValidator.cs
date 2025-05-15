namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Postback;
public class PostbackDtoValidator : AbstractValidator<PostbackDto>
{
    public PostbackDtoValidator()
    {
        RuleFor(x => x.postbackUrl).NotNull().NotEmpty().WithMessage("ValuPostbackUrl");
    }
}
