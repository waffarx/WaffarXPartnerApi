using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.ServiceInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation;
public class ValuService : IValuService
{
    public Task<Guid> SearchProduct(ProductSearchDto productSearch)
    {
        try
        {
            return new Task<Guid>(() =>
            {
                // Simulate some work
                Thread.Sleep(1000);
                return Guid.NewGuid();
            });
        }
        catch(Exception)
        {
            throw;
        }
    }
}
