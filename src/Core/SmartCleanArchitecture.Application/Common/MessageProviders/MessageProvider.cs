using SmartCleanArchitecture.Application.Common.Enums;
using SmartCleanArchitecture.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.MessageProviders
{
    public class MessageProvider : IMessageProvider
    {
        private readonly IMessageFullProvider _provider;

        public MessageProvider(IMessageFullProvider provider)
        {
            _provider = provider;
        }
        public string GetMessage(string code, string languageCode = "en")
        {
            var bundle = _provider.GetPack(languageCode);
            if (bundle == null)
            {
                throw new Exception("Invalid language configuration");
            }
            if (bundle.Mappings.TryGetValue(code, out var message))
            {
                return message;
            }
            return bundle.DefaultMessage;
        }

        public string GetOtpMessage(OtpPurpose otpPurpose)
        {
            var bundle = _provider.GetPack();
            if (bundle == null)
            {
                throw new Exception("Invalid language configuration");
            }
            if (!bundle.NotificationMessages.TryGetValue(otpPurpose, out var message))
            {
                throw new KeyNotFoundException($"No message found for the specified key - {otpPurpose}");
            }
            return message;
        }
    }
}
