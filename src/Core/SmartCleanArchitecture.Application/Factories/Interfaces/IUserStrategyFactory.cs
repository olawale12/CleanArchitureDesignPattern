using SmartCleanArchitecture.Application.Factories.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Factories.Interfaces
{
    public interface IUserStrategyFactory
    {
        UserStrategyService UserBaseOnStrategy(string countryId);
    }
}
