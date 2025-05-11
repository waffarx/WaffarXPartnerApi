namespace WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
public class UserSearchRequestModel
{
    public string Email { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int ClientApiId { get; set; }
}
