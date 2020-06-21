using RO.Chat.IO.Domain.Usuario.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RO.Chat.IO.Domain.Usuario.Entities
{
    public class Usuario
    {
        public Usuario(Guid id_Usuario, string email, string nomeUsuario, string nome, string senha)
        {
            Id_Usuario = id_Usuario;
            Email = email;
            NomeUsuario = nomeUsuario;
            Nome = nome;
            Senha = senha;
            _erros = new List<string>();
        }

        protected Usuario() {
            
        }

        public Guid Id_Usuario { get; private set; }
        public string Email { get; private set; }
        public string NomeUsuario { get; private set; }
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public ICollection<Mensagem.Entities.Mensagem> Mensagens { get; set; }
        public ICollection<Mensagem.Entities.Mensagem> Mensagens_Remetente { get; set; }
        public ICollection<Mensagem.Entities.Mensagem> Mensagens_Destino { get; set; }

        private IList<string> _erros;

        public IReadOnlyCollection<string> Erros { get { return _erros.ToArray(); } }

        public bool IsValid 
        {
            get 
            {
                Validar();
                return _erros.Count == 0;
            }

        }
        private void Validar()
        {
            _erros = _erros == null ? new List<string>() : _erros;

            if (string.IsNullOrEmpty(Email))
                AdicionarErro("E-mail não preenchido!");

            if (!Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                AdicionarErro("E-mail inválido!");

            if (string.IsNullOrEmpty(NomeUsuario))
                AdicionarErro("Nome de usuário não preenchido!");

            if (string.IsNullOrEmpty(Nome))
                AdicionarErro("Nome não preenchido!");

            if (string.IsNullOrEmpty(Senha))
                AdicionarErro("Senha não preenchida!");

            if ((new Guid()).Equals(Id_Usuario))
                AdicionarErro("Senha não preenchida!");

        }

        public void AdicionarErro(string erro)
        {
            _erros.Add(erro);
        }

        public void CriptografarSenha()
        {
            Senha = Criptografia.GerarHash(Senha);
        }
    }
}







