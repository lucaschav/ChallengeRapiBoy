using AutoMapper;
using Shared.Models;
using Shared.shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.shared.Mapping
{
    public class RolProfile : Profile
    {
        public RolProfile()
        {
            CreateMap<Rol, RolDto>().ReverseMap();
        }
    }
}
