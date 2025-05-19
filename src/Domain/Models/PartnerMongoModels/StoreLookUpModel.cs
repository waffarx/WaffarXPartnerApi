namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class StoreLookUpModel
{
    public Guid Id { get; set; }
    public int StoreId { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string Logo { get; set; }
    public string LogoPng { get; set; }
}
