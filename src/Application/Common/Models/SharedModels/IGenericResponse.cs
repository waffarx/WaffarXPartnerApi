namespace WaffarXPartnerApi.Application.Common.Models.SharedModels;
public class IGenericResponse<T>
{
    public string Status { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}
