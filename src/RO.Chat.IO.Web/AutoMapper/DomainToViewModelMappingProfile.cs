using AutoMapper;
using RO.Chat.IO.Domain.Usuario.Entities;
using RO.Chat.IO.Web.Models;

namespace RO.Chat.IO.Web.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Usuario, RegistrarViewModel>();
            CreateMap<Usuario, UsuarioViewModel>();
        }

    }
}
