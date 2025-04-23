namespace WaffarXPartnerApi.Application.Common.DTOs;
public class ApiClientDto
{
    public int ClientId { get; set; }
    public string Clientkey { get; set; } = null!;
    public string Id { get; set; }
    public string ClientName { get; set; }
    public string Secret { get; set; }
    public bool? IsActive { get; set; }
}
