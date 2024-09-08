using AutoMapper;
using SmartCleanArchitecture.Domain.Entities.Dals;
using SmartCleanArchitecture.Domain.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<Users, UsersDto>();
           
        }


    }
}
