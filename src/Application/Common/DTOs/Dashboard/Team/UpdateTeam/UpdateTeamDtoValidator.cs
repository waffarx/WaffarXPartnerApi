namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.UpdateTeam;
public class UpdateTeamDtoValidator : AbstractValidator<UpdateTeamDto>
{
    public UpdateTeamDtoValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("Team ID is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name at least 2 characters.");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
        RuleFor(x => x.PageActionIds)
            .NotEmpty().WithMessage("PageActionIds must have at least 1 item.");
    }
}
