using System.Collections.Generic;
using System.Linq;

namespace RO.Chat.IO.Domain.Mensagem.Entities
{
    public class Grupo
    {
        public Grupo(string nome)
        {
            Nome = nome;
            InstanciarLogErros();
        }

        public Grupo(int id_Grupo, string nome)
        {
            Id_Grupo = id_Grupo;
            Nome = nome;
            InstanciarLogErros();
        }

        protected Grupo() { }

        private void InstanciarLogErros()
        {
            if(_erros == null)
                _erros = new List<string>();
        }

        public int Id_Grupo { get; private set; }
        public string Nome { get; private set; }
        public ICollection<Mensagem> Mensagens { get; set; }

        private IList<string> _erros;

        public IReadOnlyCollection<string> Erros { get { return _erros.ToArray(); } }

        public void IsValid()
        {
            if (string.IsNullOrEmpty(Nome))
                AdicionarMensagemErro("Nome do grupo não preenchido!");
        }

        public void AdicionarMensagemErro(string msg)
        {
            _erros.Add(msg);
        }
    }
}
