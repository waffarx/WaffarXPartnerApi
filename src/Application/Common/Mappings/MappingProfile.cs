using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.UpdateFeatured;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;

namespace WaffarXPartnerApi.Application.Common.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateTeamDto, CreateTeamWithActionModel>();
        CreateMap<UpdateTeamDto, UpdateTeamWithActionModel>();
        CreateMap<TeamModel, TeamDto>();
        CreateMap<TeamDetailsModel, TeamDetailsDto>();
        CreateMap<UserModel, TeamUserDto>();
        CreateMap<UpdateFeaturedProductDto, UpdateFeaturedProductModel>();
    }
}
