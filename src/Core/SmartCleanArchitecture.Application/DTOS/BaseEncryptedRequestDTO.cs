using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCleanArchitecture.Application.DTOs
{
    public class BaseEncryptedRequestDTO
    {
        public string EncryptedData { get; set; }
        public bool IsValid(out string problemSource)
        {
            problemSource = string.Empty;

            if (string.IsNullOrEmpty(EncryptedData))
            {
                problemSource = "Encrypted Data";
                return false;
            }

            return true;
        }
    }
}
