using RO.Chat.IO.Domain.Interfaces;
using RO.Chat.IO.Domain.Mensagem.Entities;
using System;
using System.Collections.Generic;

namespace RO.Chat.IO.Domain.Mensagem.Interfaces
{
    public interface IMensagemRepository : IRepository<Entities.Mensagem>
    {
        Grupo AdicionarGrupo(Grupo grupo);
        Grupo ObterGrupoPorId(int id_Grupo);
        Grupo ObterGrupoPorNome(string nomeGrupo);
        List<Grupo> ObterTodosGrupos();
        List<Entities.Mensagem> ObterMensagemUsuario(Guid idUsuarioConversa, Guid idUsuarioConectado);
        List<Entities.Mensagem> ObterMensagemGrupo(int id_Grupo);
    }
}
