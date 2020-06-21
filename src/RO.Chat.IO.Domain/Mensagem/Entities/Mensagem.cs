using System;
using System.Collections.Generic;
using System.Linq;

namespace RO.Chat.IO.Domain.Mensagem.Entities
{
    public class Mensagem
    {
        protected Mensagem()
        {

        }
        public Mensagem(string texto_Mensagem, Usuario.Entities.Usuario usuario, Grupo grupo) 
        {
            Particular = false;
            Usuario = usuario;
            Grupo = grupo;
            Data_Criacao =  DateTime.Now;
            _erros = new List<string>();
            Texto_Mensagem = texto_Mensagem;
        }

        public Mensagem(string texto_Mensagem, Usuario.Entities.Usuario usuario, Usuario.Entities.Usuario usuario_Remetente, Usuario.Entities.Usuario usuario_Destino, DateTime? data_Criacao = null)
        {
            Usuario = usuario;
            Usuario_Destino = usuario_Destino;
            Usuario_Remetente = usuario_Remetente;
            Particular = true;
            Data_Criacao = data_Criacao != null ? data_Criacao.Value : DateTime.Now;
            _erros = new List<string>();
            Texto_Mensagem = texto_Mensagem;
        }

        public int Id_Mensagem { get; private set; }
        public string Texto_Mensagem { get; private set; }
        public bool Particular { get; private set; }
        public Guid Id_Usuario { get; set; }
        public virtual Usuario.Entities.Usuario Usuario { get; set; }
        public Guid? Id_Usuario_Destino { get; set; }
        public virtual Usuario.Entities.Usuario Usuario_Destino { get; set; }
        public Guid? Id_Usuario_Remetente { get; set; }
        public virtual Usuario.Entities.Usuario Usuario_Remetente { get; set; }
        public int? Id_Grupo { get; set; }
        public virtual Grupo Grupo { get; set; }
        public DateTime Data_Criacao { get; private set; }


        private IList<string> _erros;

        public IReadOnlyCollection<string> Erros { get { return _erros.ToArray(); } }

        public void IsValid()
        {
            if (Usuario == null || !Usuario.IsValid)
                AdicionarMensagemErro("Id do usuário base inválido!");

            if (string.IsNullOrEmpty(Texto_Mensagem))
                AdicionarMensagemErro("Mensagem não preenchida!");


            if (!Particular)
            {
                if (Grupo == null || Grupo.Id_Grupo == 0)
                    AdicionarMensagemErro("Grupo não informado ou inválido!");
            }

            if (Particular)
            {
                if (Usuario_Remetente == null || !Usuario_Remetente.IsValid)
                    AdicionarMensagemErro("Id do usuário remetente inválido!");

                if (Usuario_Destino == null || !Usuario_Destino.IsValid)
                    AdicionarMensagemErro("Id do usuário destino inválido!");


                if (Usuario_Remetente.Id_Usuario == Usuario_Destino.Id_Usuario)
                    AdicionarMensagemErro("Mensagem não pode ser enviada para você mesmo!");

                if (!Usuario.Id_Usuario.Equals(Usuario_Destino.Id_Usuario) && !Usuario.Id_Usuario.Equals(Usuario_Remetente.Id_Usuario))
                    AdicionarMensagemErro("Usuário base não corresponde!");
            }


        }

        public void AdicionarMensagemErro(string msg)
        {
            _erros.Add(msg);
        }
    }
}
