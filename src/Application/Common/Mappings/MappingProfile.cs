using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.UpdateFeatured;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Page;

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
        CreateMap<PageActionModel, PageActionDto>().ForMember(src => src.Id , dest => dest.MapFrom(x => x.Id.ToString()));
        CreateMap<UserPageActionsModel, UserPageActionDto>().ForMember(src => src.PageId, dest => dest.MapFrom(x => x.PageId.ToString()));
        CreateMap<PageDetailModel, PageDetailDto>().ReverseMap();
        CreateMap<PageModel, PageDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        CreateMap<UserSearchResultDto ,UserSearchModel>().ReverseMap();

    }
}
