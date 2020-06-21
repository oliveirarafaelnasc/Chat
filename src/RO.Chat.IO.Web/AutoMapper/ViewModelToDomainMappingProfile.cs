using AutoMapper;
using RO.Chat.IO.Domain.Usuario.Entities;
using RO.Chat.IO.Web.Models;
using System;

namespace RO.Chat.IO.Web.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<RegistrarViewModel, Usuario>()
                .ConstructUsing(c => new Usuario(Guid.NewGuid(), c.Email, c.Nome_Usuario, c.Nome, c.Senha));
        }
    }
}