namespace WaffarXPartnerApi.Domain.Enums;
public enum AdminActionEnum
{
    // Whitelisted Stores actions
    ListAllStores,
    ListWhitelistStores,
    AddUpdateWhitelistStores,

    // Featured Products actions
    SearchProductWithStoreAndTerm,
    AddFeaturedProducts,
    DeleteFeaturedProduct,
    UpdateFeaturedProductDates,
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
    AddMember
}
