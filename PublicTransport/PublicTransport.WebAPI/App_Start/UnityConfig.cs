using Microsoft.Practices.Unity;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.Repositories;
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

            container.RegisterType<IRouteRepository, RouteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IStopRepository, StopRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IStopTimeRepository, StopTimeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITripRepository, TripRepository>(new HierarchicalLifetimeManager());

            container.RegisterType<ILoginService, LoginService>(new HierarchicalLifetimeManager(),
                new InjectionConstructor(typeof(PublicTransportContext)));
            container.RegisterType<LoginProvider>(new HierarchicalLifetimeManager());
            
            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}