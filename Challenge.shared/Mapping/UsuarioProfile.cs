using AutoMapper;
using Challenge.shared.RequestModels;
using Shared.Models;
using Shared.shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.shared.Mapping
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, RegisterRequest>().ReverseMap();
        }
    }
}
