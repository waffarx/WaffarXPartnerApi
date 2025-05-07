using WaffarXPartnerApi.Application.Common.Models.SharedModels;

namespace WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
public interface IUserService
{
    Task<GenericResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto model);
    Task<GenericResponse<bool>> Logout();

}

public class ResetPasswordRequestDto
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
