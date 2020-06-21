using RO.Chat.IO.Domain.Interfaces;
using RO.Chat.IO.Domain.Usuario.Entities;

namespace RO.Chat.IO.Domain.Usuario.Interfaces
{
    public interface IUsuarioRepository : IRepository<Entities.Usuario>
    {
        Entities.Usuario ValidarUsuarioUnico(Entities.Usuario usuario);
        Entities.Usuario ObterUsuarioPorEmailEhSenha(string email, string senha);
    }
}
