namespace WaffarXPartnerApi.Application.Common.DTOs;
public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsSuperAdmin { get; set; }
    public int ClientApiId { get; set; }
    public DateTime? LastLogin { get; set; }
}
