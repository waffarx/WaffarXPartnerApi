namespace WaffarXPartnerApi.Domain.Enums;
public enum AdminActionEnum
{
    // Whitelisted Stores actions
    ListWhitelistStores,
    AddUpdateWhitelistStores,

    // Featured Products actions
    SearchProductWithStoreAndTerm,
    AddFeaturedProducts,
    DeleteFeaturedProduct,
    UpdateFeaturedProduct,
    ListFeaturedProducts,

    // Rank Featured Products actions
    RankFeaturedProducts,


    // Offers Lookups actions
    CreateOffer,
    UpdateOffer,
    ListAllOffers,

    // Add/Edit Offer actions
    AssignOfferToProductsOrStores,
    UpdateOfferProductsOrStores,

    // Offers Listing actions
    ListOffers,
    UpdateOfferProductsOrStoresListing,

    // Teams actions
    ListTeams,
    CreateTeam,
    EditTeam,
    ListTeamMembers,
    DeleteTeam,

    // Create/Edit Team actions (Super admin)
    AddTeam,
    EditTeamSuperAdmin,

    // Team Members actions (Super admin)
    ListTeamMembersSuperAdmin,
    DeleteMember,
    SearchAddMember,

    // Members actions (Super admin)
    ListMembers,
    DeactivateMember,

    // Add Member action (Super admin)
    AddMember,

    // Reports Screen
    ListReportCards,
    // Postback URL actions
    GetPostbackUrl,
    SavePostbackUrl,
}
