namespace WaffarXPartnerApi.Application.Common.Models.SharedModels
{
    public class HeaderData
    {
        public string ClientId { get; set; } = string.Empty;    
        public string Signature { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;    
    }
}
