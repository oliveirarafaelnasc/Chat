using System.Collections.Generic;
using RO.Chat.IO.Domain.Interfaces;
using RO.Chat.IO.Domain.Mensagem.Entities;
using RO.Chat.IO.Domain.Mensagem.Interfaces;
using System;

namespace RO.Chat.IO.Domain.Mensagem.Services
{
    public class MensagemService : IMensagemService
    {
        private readonly IMensagemRepository _mensagemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MensagemService(IMensagemRepository mensagemRepository, IUnitOfWork unitOfWork)
        {
            _mensagemRepository = mensagemRepository;
            _unitOfWork = unitOfWork;
        }

        public Grupo AdicionarGrupo(Grupo grupo)
        {

            grupo.IsValid();

            if (grupo.Erros.Count > 0)
                return grupo;

            var gruposExistentes = _mensagemRepository.ObterGrupoPorNome(grupo.Nome);

            if (gruposExistentes != null)
            {
                grupo.AdicionarMensagemErro("Grupo já existe");
                return grupo;
            }

            grupo = _mensagemRepository.AdicionarGrupo(grupo);

            _unitOfWork.Commit();

            return grupo;
        }

        public Entities.Mensagem AdicionarMensagem(Entities.Mensagem mensagem)
        {
            mensagem.IsValid();

            if (mensagem.Erros.Count > 0)
                return mensagem;

            _mensagemRepository.Adicionar(mensagem);

            _unitOfWork.Commit();

            return mensagem;
        }

        public List<Grupo> ObterTodosGrupos()
        {
            return _mensagemRepository.ObterTodosGrupos();
        }
        public Grupo ObterGrupoPorId(int id_Grupo)
        {
            return _mensagemRepository.ObterGrupoPorId(id_Grupo);
        }
        public List<Entities.Mensagem> ObterMensagemUsuario(Guid idUsuarioConversa, Guid idUsuarioConectado)
        {
            return _mensagemRepository.ObterMensagemUsuario(idUsuarioConversa, idUsuarioConectado);
        }

        public List<Entities.Mensagem> ObterMensagemGrupo(int id_Grupo)
        {
            return _mensagemRepository.ObterMensagemGrupo(id_Grupo);
        }

        public void Dispose()
        {
            _mensagemRepository.Dispose();
        }


    }
}
