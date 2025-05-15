using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Postback;
using WaffarXPartnerApi.Application.Common.Interfaces;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Enums;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class ReportService : JWTUserBaseService, IReportService
{
    private readonly IPartnerPostbackRepository _partnerPostbackRepository;
    private readonly IAuditService _auditService;
    public ReportService(IPartnerPostbackRepository partnerPostbackRepository
    , IAuditService auditService
    , IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _partnerPostbackRepository = partnerPostbackRepository;
        _auditService = auditService;
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
}
