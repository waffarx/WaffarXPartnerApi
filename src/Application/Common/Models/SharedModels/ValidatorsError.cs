using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaffarXPartnerApi.Application.Common.Models.SharedModels;
public class ValidatorsError
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorMessageAr { get; set; }
    public ValidatorsError(string ErrorCode, string ErrorMessage, string ErrorMessageAr = "")
    {
        this.ErrorCode = ErrorCode;
        this.ErrorMessage = ErrorMessage;
        this.ErrorMessageAr = ErrorMessageAr;
    }
}
