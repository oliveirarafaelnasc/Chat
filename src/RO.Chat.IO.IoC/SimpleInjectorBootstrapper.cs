using RO.Chat.IO.Data.Context;
using RO.Chat.IO.Data.Repository;
using RO.Chat.IO.Data.UoW;
using RO.Chat.IO.Domain.Interfaces;
using RO.Chat.IO.Domain.Mensagem.Interfaces;
using RO.Chat.IO.Domain.Mensagem.Services;
using RO.Chat.IO.Domain.Usuario.Interfaces;
using RO.Chat.IO.Domain.Usuario.Services;
using SimpleInjector;

namespace RO.Chat.IO.IoC
{
    public class SimpleInjectorBootstrapper
    {
        public static void Register(Container container, ScopedLifestyle scoped)
        {

            // Service
            container.Register<IUsuarioService, UsuarioService>(Lifestyle.Scoped);
            container.Register<IMensagemService, MensagemService>(Lifestyle.Scoped);

            // Repository
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
            container.Register<IUsuarioRepository, UsuarioRepository>(Lifestyle.Scoped);
            container.Register<IMensagemRepository, MensagemRepository>(Lifestyle.Scoped);

            //Context
            container.Register<ChatContext>(Lifestyle.Singleton);
        }
    }
}
