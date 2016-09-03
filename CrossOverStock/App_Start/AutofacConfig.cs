using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using CrossOverStock.Data;
using CrossOverStock.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrossOverStock.Data.Domain.Authetication;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace CrossOverStock
{
    public class AutofacConfig
    {
        public static void RegisterAutofac()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<Services.Implement.StockExchangeSoap>().As<Services.IStockExchangeSoap>().SingleInstance();

            builder.RegisterType<StockContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<IdentityContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<Data.Impl.CurrentUser>().As<ICurrentUser>().InstancePerRequest();
            builder.RegisterType<StockUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<RoleManager<StockRole, int>>().AsSelf().InstancePerRequest();
            builder.Register(c => new UserStore<StockUser, StockRole, int, StockUserLogin, StockUserRole, StockUserClaim>(c.Resolve<IdentityContext>())).AsImplementedInterfaces().InstancePerRequest();
            builder.Register(c => new RoleStore<StockRole, int, StockUserRole>(c.Resolve<IdentityContext>())).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .WithParameter(new ResolvedParameter(
                   (pi, ctx) => pi.ParameterType == typeof(IUnitOfWork),
                   (pi, ctx) => ctx.Resolve<Data.Impl.UnitOfWork<StockContext>>()))
                .InstancePerRequest();

            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
        
    }
}