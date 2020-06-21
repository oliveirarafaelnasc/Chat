using RO.Chat.IO.Domain.Usuario.Entities;
using System;
using System.Collections.Generic;

namespace RO.Chat.IO.Domain.Usuario.Interfaces
{
    public interface IUsuarioService : IDisposable
    {
        Entities.Usuario AdicionarUsuario(Entities.Usuario usuario);
        Entities.Usuario ObterPorId(Guid id);
        List<Entities.Usuario> ObterTodos();
        Entities.Usuario ObterUsuarioPorEmailEhSenha(string email, string senha);
    }
}
