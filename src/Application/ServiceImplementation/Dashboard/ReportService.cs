using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Postback;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Report;
using WaffarXPartnerApi.Application.Common.Interfaces;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Enums;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class ReportService : JWTUserBaseService, IReportService
{
    private readonly IPartnerPostbackRepository _partnerPostbackRepository;
    private readonly IAuditService _auditService;
    private readonly IApiClientRepository _apiClientRepository;
    private readonly ICashBackRepository _cashBackRepository;
    private readonly IMapper _mapper;
    public ReportService(IPartnerPostbackRepository partnerPostbackRepository
    , IAuditService auditService, IApiClientRepository apiClientRepository
    , ICashBackRepository cashBackRepository, IMapper mapper
    , IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _partnerPostbackRepository = partnerPostbackRepository;
        _auditService = auditService;
        _apiClientRepository = apiClientRepository;
        _cashBackRepository = cashBackRepository;
        _mapper = mapper;   
    }
    public async Task<GenericResponse<bool>> AddOrUpdatePostback(PostbackDto postbackDto)
    {
        bool isUpdated = false;
        try
        {
            var result = await _partnerPostbackRepository.GetByClientIdAsync(ClientApiId);
            if (result == null)
            {
                var postback = new PartnerPostback
                {
                    ClientApiId = ClientApiId,
                    PostbackUrl = postbackDto.PostbackUrl,
                    UpdatedAt = DateTime.Now,  
                    PostbackMethod = (int)PostbackMethodEnum.POST,
                    CreatedAt = DateTime.Now,
                    PostbackType = (int)PostbackTypeEnum.Orders,
                };

                postback = await _partnerPostbackRepository.CreateAsync(postback);
                await _auditService.LogCreationAsync(new AuditCreationParams<PartnerPostback>
                {
                    EntityType = EntityTypeEnum.PartnerPostback,
                    ClientApiId = ClientApiId,
                    UserId = UserId,
                    EntityId = postback.Id,
                    Entity = postback,
                });
                isUpdated = true;
            }
            else
            {
                PartnerPostback oldData = result;
                result.PostbackUrl = postbackDto.PostbackUrl;
                isUpdated = await _partnerPostbackRepository.UpdateAsync(result);
                await _auditService.LogUpdateAsync(new AuditUpdateParams<PartnerPostback>
                {
                    OldEntity = oldData,
                    NewEntity = result,
                    EntityType = EntityTypeEnum.PartnerPostback,
                    ClientApiId = ClientApiId,
                    EntityId = result.Id
                });
            }
            return new GenericResponse<bool>
            {
                Data = isUpdated,
                Status = StaticValues.Success
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<string>> GetPartnerPostbackUrl()
    {
        try
        {
            var result = await _partnerPostbackRepository.GetByClientIdAsync(ClientApiId);
            if (result == null)
            {
                return new GenericResponse<string>
                {
                    Data = "",
                    Status = StaticValues.Success
                };
            }
            return new GenericResponse<string>
            {
                Data = result.PostbackUrl,
                Status = StaticValues.Success
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<GetParterOrderStatisticsDto>> GetPartnerOrdersStatistics()
    {
        GetParterOrderStatisticsDto dto = new GetParterOrderStatisticsDto();
        try
        {
            var userId = await _apiClientRepository.GetUserIdByClientId(ClientApiId);
            if (userId > 0)
            {
                var cashBacks = await _cashBackRepository.GetParterOrderStatistics(userId);
                return new GenericResponse<GetParterOrderStatisticsDto>
                {
                    Data = _mapper.Map<GetParterOrderStatisticsDto>(cashBacks),
                    Status = StaticValues.Success
                };
            }
            return new GenericResponse<GetParterOrderStatisticsDto>
            {
                Data = new GetParterOrderStatisticsDto(),
                Status = StaticValues.Error
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
