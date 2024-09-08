using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.Enums
{
    public class Otp
    {
    }

    public enum OtpPurpose
    {
        [Description("SS0 ONBOARDING")]
        SSO_ONBOARDING = 1,
        [Description("PASSWORD RESET")]
        PASSWORD_RESET,
        [Description("2FA")]
        TWO_FACTOR_AUTHENTICATION
    }
}
