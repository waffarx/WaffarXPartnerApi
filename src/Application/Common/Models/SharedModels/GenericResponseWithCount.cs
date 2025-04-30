namespace WaffarXPartnerApi.Application.Common.Models.SharedModels;
public class GenericResponseWithCount<T> : IGenericResponse<T>
{
   
    public int? TotalCount { get; set; }
   
}
