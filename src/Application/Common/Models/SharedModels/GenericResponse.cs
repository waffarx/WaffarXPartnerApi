using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaffarXPartnerApi.Application.Common.Models.SharedModels
{

    public class GenericResponse<T>
    {
        public string Status { get; set; }
        public T Data { get; set; }
        public int? TotalCount { get; set; }
        public string Message{ get; set; }
        public List<string> Errors { get; set; }
        public bool FromCache { get; set; }
        public string Code { get; set; }
    }
}
