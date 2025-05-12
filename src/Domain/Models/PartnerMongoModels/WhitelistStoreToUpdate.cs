namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class WhitelistStoreToUpdate
{
    public string StoreId { get; set; }
    public bool IsFeatured { get; set; }
    public int Rank { get; set; }
    public string BackgroundColor { get; set; }
}
