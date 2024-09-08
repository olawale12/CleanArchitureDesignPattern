using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.MessageProviders
{
    public class MessageFullSettings
    {
        public string BaseLocation { get; set; }
        public string DefaultMessage { get; set; }
        public string PasswordResetMessage { get; set; }
        public string OnboardingMessage { get; set; }
        public string TwoFactorAuthentication { get; set; }
    }
}
