using Microsoft.Extensions.DependencyInjection;
using SmartCleanArchitecture.Application.Factories.Abstracts;
using SmartCleanArchitecture.Application.Factories.Implementations;
using SmartCleanArchitecture.Application.Factories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Factories.Strategies
{
    public class UserStrategyFactory: IUserStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UserStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public UserStrategyService UserBaseOnStrategy(string countryId)
        {
            switch (countryId)
            {
                case "01":
                return _serviceProvider.GetService<NigeriaUserStrategyFactory>()!;
                default:
                throw new ArgumentException($"Unsupported country: {countryId}");
            }
        }
    }
}
