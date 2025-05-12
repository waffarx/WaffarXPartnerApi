namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class DetailedStoreModel : StoreModel
{
    public bool IsFeatured { get; set; }
    public int Rank { get; set; }
    public string BackgroundColor { get; set; }
}
