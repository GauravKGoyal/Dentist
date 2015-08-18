using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Dentist.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dentist
{
    public static class DependencyInjectionConfig
    {
        private static ContainerBuilder Builder { get; set; }

        public static IContainer Container { get; set; }

        public static void RegisterDependencyInjection()
        {
            if (Builder != null)
            {
                return;
            }
            
            Builder = new ContainerBuilder();
            
            RegisterObjects();

            // Set the dependency resolver to be Autofac.
            Container = Builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }

        private static void RegisterObjects()
        {
            // Register your MVC controllers.
            Builder.RegisterControllers(typeof(MvcApplication).Assembly);

            Builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Register context
            //Builder.Register(c => new ApplicationDbContext())
            //    .InstancePerLifetimeScope()
            //    .Named<ApplicationDbContext>("ReadContext");

            //Builder.Register(c => new ApplicationDbContext(true))
            //    .InstancePerLifetimeScope()
            //    .Named<ApplicationDbContext>("WriteContext");  
        }
    }
}
