using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class WhiteListStoreModel
{
    public int StoreId { get; set; }
    public bool IsFeatured { get; set; }
    public int Rank { get; set; }
}
