using AutoMapper;
using Microsoft.AspNet.SignalR;
using RO.Chat.IO.Domain.Mensagem.Entities;
using RO.Chat.IO.Domain.Mensagem.Interfaces;
using RO.Chat.IO.Domain.Usuario.Entities;
using RO.Chat.IO.Domain.Usuario.Interfaces;
using RO.Chat.IO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace RO.Chat.IO.Web.Hubs
{
    [Authorize]
    public class RO_ChatHub : Hub
    {
		

        public readonly static List<UsuarioViewModel> _usuariosConectados = new List<UsuarioViewModel>();
        private readonly static Dictionary<Guid, string> _mapaUsuariosConectados = new Dictionary<Guid, string>();

        private readonly IMensagemService _mensagemService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public RO_ChatHub(IUsuarioService usuarioService, IMensagemService mensagemService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mensagemService = mensagemService;
            _mapper = mapper;
            
        }

        public int AdicionarNovoGrupo(string nomeGrupo)
        {
            try
            {
                Grupo grupo = new Grupo(nomeGrupo);
                _mensagemService.AdicionarGrupo(grupo);

                UsuarioGrupoChatViewModel usuarioGrupoChatViewModel = new UsuarioGrupoChatViewModel()
                {
                    Id = grupo.Id_Grupo.ToString(),
                    Nome = grupo.Nome,
                    NomeChave = string.Empty,
                    NomeUsuario = grupo.Nome,
                    IsGrupo = true
                };

                Clients.All.novoGrupo(usuarioGrupoChatViewModel);
                return grupo.Id_Grupo;
            }
            catch (Exception ex)
            {
                Clients.Caller.onError(string.Format("falha ao adicionar grupo!Erro.: {0}",
                    ex?.InnerException?.Message, ex.Message
                    ));

                return 0;
            }
        }

        public List<UsuarioGrupoChatViewModel> ObterUsuarios()
        {
            Guid idUsuarioLogado = IdentityIdUser;
            List<Grupo> grupos = _mensagemService.ObterTodosGrupos();
            List<Usuario> usuarios = _usuarioService.ObterTodos();


            List<UsuarioGrupoChatViewModel> usuarioGrupoChatViewModels = new List<UsuarioGrupoChatViewModel>();

            usuarioGrupoChatViewModels = (from u in usuarios
                                          where u.Id_Usuario != idUsuarioLogado
                                          select new UsuarioGrupoChatViewModel
                                          {
                                              Id = u.Id_Usuario.ToString(),
                                              Nome = u.Nome,
                                              NomeChave = u.Email,
                                              NomeUsuario = u.NomeUsuario,
                                              IsGrupo = false
                                          }
                                          )
                                          .ToList();



            usuarioGrupoChatViewModels.AddRange(from g in grupos
                                                select new UsuarioGrupoChatViewModel
                                                {
                                                    Id = g.Id_Grupo.ToString(),
                                                    Nome = g.Nome,
                                                    NomeChave = string.Empty,
                                                    NomeUsuario = g.Nome,
                                                    IsGrupo = true
                                                });

            return usuarioGrupoChatViewModels;
        }


        public IEnumerable<MensagemViewModel> ObterMensagensPorUsuario(Guid idUsuario)
        {
            List<MensagemViewModel> msgs = new List<MensagemViewModel>();
            if (idUsuario.Equals(IdentityName))
                return msgs;

            Usuario usuarioConversa = _usuarioService.ObterPorId(idUsuario);

            var mensagemParticulares = _mensagemService.ObterMensagemUsuario(usuarioConversa.Id_Usuario, IdentityIdUser);

            if (mensagemParticulares == null)
                return new List<MensagemViewModel>();

            foreach (var msgPar in mensagemParticulares)
            {
                MensagemViewModel mensagemParticularViewModel = new MensagemViewModel()
                {
                    Texto = msgPar.Texto_Mensagem,
                    DataMensagem = msgPar.Data_Criacao.ToString("dd/MM/yyyy HH:mm:ss"),
                    Id_Usuario_Mensagem  = msgPar.Usuario_Remetente.Id_Usuario,
                    NomeUsuario = msgPar.Usuario_Remetente.NomeUsuario,
                    Login = msgPar.Usuario_Remetente.Email

                };

                msgs.Add(mensagemParticularViewModel);
            }

            return msgs;
        }

        public IEnumerable<MensagemViewModel> ObterMensagensPorGrupo(int id_Grupo)
        {
            List<MensagemViewModel> msgs = new List<MensagemViewModel>();

            Grupo grupo = _mensagemService.ObterGrupoPorId(id_Grupo);
            List<Mensagem> mensagens = _mensagemService.ObterMensagemGrupo(id_Grupo);

            if (mensagens == null)
                return new List<MensagemViewModel>();

            foreach (var msgPar in mensagens)
            {
                MensagemViewModel mensagemParticularViewModel = new MensagemViewModel()
                {
                    Texto = msgPar.Texto_Mensagem,
                    DataMensagem = msgPar.Data_Criacao.ToString("dd/MM/yyyy HH:mm:ss"),
                    Id_Usuario_Mensagem = msgPar.Usuario.Id_Usuario,
                    NomeUsuario = msgPar.Usuario.NomeUsuario,
                    Login = msgPar.Usuario.Email,
                    Nome_Grupo = grupo.Nome,
                    Id_Grupo = grupo.Id_Grupo

                };

                msgs.Add(mensagemParticularViewModel);
            }

            return msgs;
        }

        public void EnviarMensagemUsuario(Guid idUsuarioConversa, string mensagem)
        {
            try
            {

                Usuario usuarioConvesa = _usuarioService.ObterPorId(idUsuarioConversa);
                Usuario usuarioremetente = _usuarioService.ObterPorId(IdentityIdUser);
            
                DateTime dataEnvio = DateTime.Now;
                Mensagem minhaMensagem = new Mensagem(mensagem, usuarioremetente, usuarioremetente, usuarioConvesa, dataEnvio);
                Mensagem mensagemDestino = new Mensagem(mensagem, usuarioConvesa, usuarioremetente, usuarioConvesa, dataEnvio);

                _mensagemService.AdicionarMensagem(minhaMensagem);
                _mensagemService.AdicionarMensagem(mensagemDestino);

                 MensagemViewModel mensagemViewModel = new MensagemViewModel()
                {
                    Texto = mensagemDestino.Texto_Mensagem,
                    DataMensagem = mensagemDestino.Data_Criacao.ToString("dd/MM/yyyy HH:mm:ss"),
                    Id_Usuario_Destino = idUsuarioConversa,
                    Id_Usuario_Mensagem = mensagemDestino.Usuario_Remetente.Id_Usuario,
                    NomeUsuario = mensagemDestino.Usuario_Remetente.NomeUsuario,
                    Login = mensagemDestino.Usuario_Remetente.Email,
                    Id_Grupo = 0

                };


                //Para algum usuario especifico
               /*
                var usuario = _mapaUsuariosConectados.Where(w => w.Key == usuarioConvesa.Id_Usuario).Select(s => s).FirstOrDefault();
                if (usuario.Value != null)
                {
                    Clients.Client(usuario.Value).novaMensagem(mensagemViewModel);
                }
               */
                Clients.All.novaMensagem(mensagemViewModel);


            }
            catch (Exception ex)
            {
                Clients.Caller.onError(string.Format("falha ao enviar mensagem!Erro.: {0}",
                    ex?.InnerException?.Message, ex.Message));
            }
        }

        public void EnviarMensagemGrupo(int idgrupo, string mensagem)
        {
            try
            {

                Grupo grupo = _mensagemService.ObterGrupoPorId(idgrupo);
                Usuario usuarioremetente = _usuarioService.ObterPorId(IdentityIdUser);

                DateTime dataEnvio = DateTime.Now;
                Mensagem minhaMensagem = new Mensagem(mensagem, usuarioremetente, grupo);

                _mensagemService.AdicionarMensagem(minhaMensagem);

                MensagemViewModel mensagemViewModel = new MensagemViewModel()
                {
                    Texto = minhaMensagem.Texto_Mensagem,
                    DataMensagem = minhaMensagem.Data_Criacao.ToString("dd/MM/yyyy HH:mm:ss"),
                    Id_Usuario_Mensagem = usuarioremetente.Id_Usuario,
                    NomeUsuario = usuarioremetente.NomeUsuario,
                    Login = usuarioremetente.Email,
                    Id_Grupo = idgrupo,
                    Nome_Grupo = grupo.Nome

                };
                //Para algum usuario especifico
                // var usuario = _mapaUsuariosConectados.Where(w => w.Key == usuarioConvesa.Id_Usuario).Select(s => s).FirstOrDefault();
                // Clients.Client(usuario.Value).novaMensagemGrupo(mensagemViewModel);

                Clients.All.novaMensagemGrupo(mensagemViewModel);


            }
            catch (Exception ex)
            {
                Clients.Caller.onError(string.Format("falha ao enviar mensagem!Erro.: {0}",
                    ex?.InnerException?.Message, ex.Message));
            }
        }

        public override Task OnConnected()
        {
            try
            {
                Clients.Caller.marcarUsuarioLogado(IdentityIdUser);

                var usuario = _usuarioService.ObterPorId(IdentityIdUser);
                UsuarioViewModel usuarioViewModel = _mapper.Map<UsuarioViewModel>(usuario);
                UsuarioGrupoChatViewModel usuarioGrupoChatViewModel = new UsuarioGrupoChatViewModel()
                {
                    Id = usuario.Id_Usuario.ToString(),
                    Nome = usuario.Nome,
                    NomeChave = usuario.Email,
                    NomeUsuario = usuario.NomeUsuario,
                    IsGrupo = false
                };


                if (!_usuariosConectados.Exists(w => w.Id_Usuario == IdentityIdUser))
                    _usuariosConectados.Add(usuarioViewModel);

                Thread.Sleep(1000);

                if (!_mapaUsuariosConectados.Any(w => w.Key == IdentityIdUser))
                    _mapaUsuariosConectados.Add(IdentityIdUser, Context.ConnectionId);

                Clients.All.informarQuemConectou(usuarioGrupoChatViewModel);

            }
            catch (Exception ex)
            {

                Clients.Caller.onError(string.Format("Falha ao conectar!Erro.: {0}",
                    ex?.InnerException?.Message, ex.Message));
            }


            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {

                var usuario = _usuarioService.ObterPorId(IdentityIdUser);

                UsuarioGrupoChatViewModel usuarioGrupoChatViewModel = new UsuarioGrupoChatViewModel()
                {
                    Id = usuario.Id_Usuario.ToString(),
                    Nome = usuario.Nome,
                    NomeChave = usuario.Email,
                    NomeUsuario = usuario.NomeUsuario,
                    IsGrupo = false
                };

                Thread.Sleep(1000);

                Clients.All.informarQuemSaiu(usuarioGrupoChatViewModel);

            }
            catch (Exception ex)
            {
                Clients.Caller.onError(string.Format("Falha ao desconectar!Erro.: {0}",
                    ex?.InnerException?.Message, ex.Message));
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            try
            {
                var usuario = _usuarioService.ObterPorId(IdentityIdUser);
                UsuarioViewModel usuarioViewModel = _mapper.Map<UsuarioViewModel>(usuario);

                Clients.All.informarQuemConectou(usuarioViewModel);

            }
            catch (Exception ex)
            {
                Clients.Caller.onError(string.Format("Falha ao reconectar!Erro.: {0}",
                    ex?.InnerException?.Message, ex.Message));
            }
            return base.OnReconnected();
        }

        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }

        private IEnumerable<Claim> IdentityClaims
        {
            get {
                var identity = (ClaimsIdentity)Context.User.Identity;
                return identity.Claims;

            }
        }

        private Guid IdentityIdUser
        {
            get
            {
                IEnumerable<Claim> claims = IdentityClaims;
                Claim claim = claims.Where(w => w.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                Guid idUser = new Guid();

                Guid.TryParse(claim.Value, out idUser);

                return idUser;

            }
        }
    }
}