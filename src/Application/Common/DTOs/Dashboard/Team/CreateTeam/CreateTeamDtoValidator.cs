namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.CreateTeam;
public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name at least 2 characters.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(2).WithMessage("Name at least 2 characters.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        RuleFor(x => x.PageActionIds)
            .NotEmpty().WithMessage("PageActionIds must have at least 1 item.");
    }
}
