using RO.Chat.IO.Data.Context;
using RO.Chat.IO.Domain.Mensagem.Entities;
using RO.Chat.IO.Domain.Mensagem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RO.Chat.IO.Data.Repository
{

    public class MensagemRepository : Repository<Mensagem>, IMensagemRepository
    {
        public MensagemRepository(ChatContext context) : base(context)
        {

        }
        public Grupo AdicionarGrupo(Grupo grupo)
        {
            return Db.Grupos.Add(grupo);
        }

        public Grupo ObterGrupoPorId(int id_Grupo)
        {
            return Db.Grupos.Where(w => w.Id_Grupo == id_Grupo).FirstOrDefault();
        }

        public Grupo ObterGrupoPorNome(string nomeGrupo)
        {
            return Db.Grupos.Where(w => w.Nome == nomeGrupo).FirstOrDefault();
        }

        public List<Mensagem> ObterMensagemGrupo(int id_Grupo)
        {
            return Db.Mensagens
                           .Include("Usuario")
                           .Include("Grupo")
                           .AsNoTracking()
                           .Where(w => w.Grupo.Id_Grupo == id_Grupo && !w.Particular)
                           .ToList();
        }

        public List<Mensagem> ObterMensagemUsuario(Guid idUsuarioConversa, Guid idUsuarioConectado)
        {
            return Db.Mensagens
                .Include("Usuario")
                .Include("Usuario_Remetente")
                .Include("Usuario_Destino")
                .AsNoTracking()
                .Where(w => w.Usuario.Id_Usuario == idUsuarioConectado && w.Particular && 
                ( w.Usuario_Remetente.Id_Usuario == idUsuarioConversa || w.Usuario_Destino.Id_Usuario == idUsuarioConversa))
                .ToList();
        }

        public List<Grupo> ObterTodosGrupos()
        {
            return Db.Grupos.ToList();
        }
    }
}
