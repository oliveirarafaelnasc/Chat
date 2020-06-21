using Dapper;
using RO.Chat.IO.Data.Context;
using RO.Chat.IO.Domain.Usuario.Entities;
using RO.Chat.IO.Domain.Usuario.Interfaces;
using System.Linq;

namespace RO.Chat.IO.Data.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ChatContext context) : base(context)
        {

        }

        public Usuario ObterUsuarioPorEmailEhSenha(string email, string senha)
        {
            var sql = @"select top 1 *
                        from dbo.Usuario with(nolock)
                        where email = @email and senha = @senha";

            return Db.Database.Connection.Query<Usuario>
                (sql,
                new { email = email, senha = senha }
                ).Select(s => s).FirstOrDefault();
        }

        public Usuario ValidarUsuarioUnico(Usuario usuario)
        {
            var sql = @"select top 1 *
                        from dbo.Usuario with(nolock)
                        where (NomeUsuario = @nomeUsuario or  email = @email )";

            return Db.Database.Connection.Query<Usuario>
                (sql, 
                new { nomeUsuario = usuario.NomeUsuario, email = usuario.Email }
                ).Select(s => s).FirstOrDefault();
        }

    }
}
