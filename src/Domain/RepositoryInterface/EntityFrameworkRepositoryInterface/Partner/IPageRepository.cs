namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
public interface IPageRepository
{
    Task<List<PageDetailModel>> GetPagesWithAction(int clientApiId);

}

public class PageDetailModel
{
    public List<PageModel> Pages { get; set; }

}
public class PageModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<PageActionModel> PageActions { get; set; }
}
public class PageActionModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
