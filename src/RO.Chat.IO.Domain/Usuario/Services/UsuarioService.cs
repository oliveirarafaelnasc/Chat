using RO.Chat.IO.Domain.Interfaces;
using RO.Chat.IO.Domain.Usuario.Interfaces;
using RO.Chat.IO.Domain.Usuario.ValueObject;
using System;
using System.Collections.Generic;

namespace RO.Chat.IO.Domain.Usuario.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UsuarioService(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }
        public Entities.Usuario AdicionarUsuario(Entities.Usuario usuario)
        {
            
            if (!usuario.IsValid)
                return usuario;

            var usuarioExistente = _usuarioRepository.ValidarUsuarioUnico(usuario);

            if (usuarioExistente != null)
                usuario.AdicionarErro("Nome de Usuario ou email inválidos para inclusão!");

            if (usuario.Erros.Count > 0)
                return usuario;

            usuario.CriptografarSenha();

            _usuarioRepository.Adicionar(usuario);

            int resultado =  _unitOfWork.Commit();

            if(resultado <= 0)
                usuario.AdicionarErro("Não foi possivel registrar o usuario!");

            return usuario;
        }
        public Entities.Usuario ObterUsuarioPorEmailEhSenha(string email, string senha)
        {
            string senhaCriptograda = Criptografia.GerarHash(senha);
            Entities.Usuario usuario = _usuarioRepository.ObterUsuarioPorEmailEhSenha(email, senhaCriptograda);

            if (usuario == null)
            {
                usuario = new Entities.Usuario(new Guid(), email, string.Empty, string.Empty, senha);
                usuario.AdicionarErro("Dados inválidos!");
            }

            return usuario;
        }
        public Entities.Usuario ObterPorId(Guid id)
        {
            return _usuarioRepository.ObterPorId(id);
        }

        public List<Entities.Usuario> ObterTodos()
        {
            return _usuarioRepository.ObterTodos();
        }
        public void Dispose()
        {
            _usuarioRepository.Dispose();
        }


    }
}
