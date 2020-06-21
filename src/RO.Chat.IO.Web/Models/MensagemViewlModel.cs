using System;

namespace RO.Chat.IO.Web.Models
{
    public class MensagemViewModel
    {
        public string Texto { get; set; }
        public string DataMensagem { get; set; }
        public Guid Id_Usuario_Mensagem { get; set; }
        public string NomeUsuario { get; set; }
        public string Login { get; set; }
        public Guid Id_Usuario_Destino { get; set; }
        public int Id_Grupo { get; set; }
        public string Nome_Grupo { get; set; }

    }
}