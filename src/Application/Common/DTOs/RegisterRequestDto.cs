namespace WaffarXPartnerApi.Application.Common.DTOs;
public class RegisterRequestDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> TeamsIds { get; set; }

}
