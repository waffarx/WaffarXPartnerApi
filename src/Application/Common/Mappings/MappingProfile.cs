using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.UpdateFeatured;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Page;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.CreateTeam;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.UpdateTeam;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Report;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferType;

namespace WaffarXPartnerApi.Application.Common.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateTeamDto, CreateTeamWithActionModel>();
        CreateMap<UpdateTeamDto, UpdateTeamWithActionModel>();
        CreateMap<TeamModel, TeamDto>().ReverseMap();
        CreateMap<TeamDetailsModel, TeamDetailsDto>();
        CreateMap<UserModel, TeamUserDto>();
        CreateMap<UpdateFeaturedProductDto, UpdateFeaturedProductModel>();
        CreateMap<PageActionModel, PageActionDto>().ForMember(src => src.Id , dest => dest.MapFrom(x => x.Id.ToString()));
        CreateMap<UserPageActionsModel, UserPageActionDto>().ForMember(src => src.PageId, dest => dest.MapFrom(x => x.PageId.ToString()));
        CreateMap<PageDetailModel, PageDetailDto>().ReverseMap();
        CreateMap<PageModel, PageDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        CreateMap<UserSearchResultDto ,UserSearchModel>().ReverseMap();
        CreateMap<UserDetailModel, UserDetailDto>();
        CreateMap<GetParterOrderStatisticsModel, GetParterOrderStatisticsDto>();
        CreateMap<OfferTypeModel, OfferTypeResponsetDto>();

    }
}
