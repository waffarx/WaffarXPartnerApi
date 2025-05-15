namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

public partial class ApiClientExitClick
{
    public int Id { get; set; }

    /// <summary>
    /// ClientId In ApiClient Table
    /// </summary>
    public int ApiClientId { get; set; }

    /// <summary>
    /// Id of Client AppUser
    /// </summary>
    public int AppUserId { get; set; }

    public long ExitClickId { get; set; }

    /// <summary>
    /// Just Flag Setted by partner to tell if the ExitClick has cashback or not
    /// </summary>
    public bool HasCashback { get; set; }

    /// <summary>
    /// Identifier Sent From Partner
    /// </summary>
    public string Uid { get; set; }

    public string SubId1 { get; set; }

    public string SubId2 { get; set; }
}
