using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class UpdateFeaturedProductModel
{
    public string ProductId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }
    public int ClientApiId { get; set; }
}
