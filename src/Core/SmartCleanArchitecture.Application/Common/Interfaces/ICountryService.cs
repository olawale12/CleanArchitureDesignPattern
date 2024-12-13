using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.Interfaces
{
    public interface ICountryService
    {
        void SetCountryCode(string countryCode);
        string GetCountryCode();
    }
}
