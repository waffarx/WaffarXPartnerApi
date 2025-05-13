namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User.AssignUserToTeam;
public class AssignUserToTeamRequestDtoValidator : AbstractValidator<AssignUserToTeamRequestDto>
{
    public AssignUserToTeamRequestDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
        RuleFor(x => x.TeamIds)
            .NotEmpty()
            .WithMessage("At least one Team ID is required.");
    }
}
