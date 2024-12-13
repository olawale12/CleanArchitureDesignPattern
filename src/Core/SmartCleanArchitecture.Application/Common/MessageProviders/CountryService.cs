using SmartCleanArchitecture.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.MessageProviders
{
    public class CountryService : ICountryService
    {
        private string _countryCode;

        public string GetCountryCode()
        {
            return _countryCode;
        }

        public void SetCountryCode(string countryCode)
        {
            _countryCode = countryCode;
        }
    }
}
