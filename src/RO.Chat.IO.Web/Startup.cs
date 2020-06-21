using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using RO.Chat.IO.IoC;
using RO.Chat.IO.Web.AutoMapper;
using RO.Chat.IO.Web.Hubs;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(RO.Chat.IO.Web.Startup))]

namespace RO.Chat.IO.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ChatCookie",
                ExpireTimeSpan = new TimeSpan(0, 0, 45, 0),
                LoginPath = new PathString("/Account/Register")
            });

            var container = new Container();

            var hybrid = Lifestyle.CreateHybrid(
                () => container.BeginExecutionContextScope() != null,
                new ExecutionContextScopeLifestyle(),
                new SimpleInjector.Integration.Web.WebRequestLifestyle());
                container.Options.DefaultScopedLifestyle = hybrid;

            SimpleInjectorBootstrapper.Register(container, hybrid);
            container.RegisterSingleton(AutoMapperConfiguration.RegisterMappings);
            var activator = new SimpleInjectorHubActivator(container);
            container.Register<RO_ChatHub>(Lifestyle.Scoped);

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => activator);
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            app.MapSignalR(container.GetInstance<HubConfiguration>());

        }
    }

    public class SimpleInjectorHubActivator : IHubActivator
    {
        private readonly Container _container;

        public SimpleInjectorHubActivator(Container container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)_container.GetInstance(descriptor.HubType);
        }
    }

    public class SimpleInjectorSignalRDependencyResolver : DefaultDependencyResolver
    {
        public SimpleInjectorSignalRDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var @this = (IEnumerable<object>)_serviceProvider.GetService(
                typeof(IEnumerable<>).MakeGenericType(serviceType));

            var @base = base.GetServices(serviceType);

            return @this == null ? @base : @base == null ? @this : @this.Concat(@base);
        }

        private readonly IServiceProvider _serviceProvider;
    }
}
