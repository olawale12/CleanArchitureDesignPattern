using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Models
{
    public record SystemSetting
    {
        public string CountryId { get; set; } = string.Empty;
        public string BaseAddress { get; set; } = string.Empty;
        public int Timeout { get; set; }
        public string EncryptionKey { get; set; }
        public bool EncryptResponseData { get; set; }
        public bool UseEncryptedData { get; set; }
        public int EncryptResponseDataBufferSize { get; set; }
        public bool AddBufferSize { get; set; }




    }
}
