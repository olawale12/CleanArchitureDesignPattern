using SmartCleanArchitecture.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.Interfaces
{
    public interface IMessageProvider
    {
        string GetMessage(string code, string languageCode = "en");
        string GetOtpMessage(OtpPurpose otpPurpose);
    }
}
