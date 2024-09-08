using SmartCleanArchitecture.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.MessageProviders
{
    public class MessageFull
    {
        public MessageFull(string defaultMessage)
        {
            DefaultMessage = defaultMessage;
        }
        public string DefaultMessage { get; set; }
        public IDictionary<string, string> Mappings { get; set; }
        public IDictionary<OtpPurpose, string> NotificationMessages { get; set; }
    }
}
