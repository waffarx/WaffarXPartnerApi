using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Page;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class CommenService : JWTUserBaseService, ICommonService
{
    private readonly IPageRepository _pageRepository;
    private readonly IMapper _mapper;
    public CommenService(IHttpContextAccessor httpContextAccessor, IPageRepository pageRepository, IMapper mapper) : base(httpContextAccessor)
    {
        _pageRepository = pageRepository;
        _mapper = mapper;
    }

    public async Task<GenericResponse<List<PageDetailDto>>> GetPages()
    {
        try
        {
           var pagesToGet =  await _pageRepository.GetPagesWithAction(ClientApiId);
            if (pagesToGet == null) 
            {
                return new GenericResponse<List<PageDetailDto>>
                {
                    Data = new List<PageDetailDto>(),
                    Status = StaticValues.Error,
                };
            }
            return new GenericResponse<List<PageDetailDto>> 
            {
                Data = _mapper.Map<List<PageDetailDto>>(pagesToGet.ToList()),
                Status = StaticValues.Success,
            };
        }
        catch (Exception) 
        {
            throw;
        }
    }
}
