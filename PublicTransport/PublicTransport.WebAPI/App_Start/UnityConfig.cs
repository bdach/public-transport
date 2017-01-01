using Microsoft.Practices.Unity;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services;
using PublicTransport.Services.Contracts;
using PublicTransport.WebAPI.Identity;
using Unity.WebApi;

namespace PublicTransport.WebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
			var container = new UnityContainer();
            
            container.RegisterType<PublicTransportContext>(new HierarchicalLifetimeManager(), new InjectionConstructor());
            container.RegisterType<ILoginService, LoginService>(new HierarchicalLifetimeManager(),
                new InjectionConstructor(typeof(PublicTransportContext)));
            container.RegisterType<LoginProvider>(new HierarchicalLifetimeManager());
            
            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}