namespace WaffarXPartnerApi.Domain.Models.SharedModels;
public class PaginationResultModel<T>
{
    public int TotalRecords { get; set; }
    public T Data { get; set; }
}
