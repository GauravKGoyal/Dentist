using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dentist
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DependencyInjectionConfig.RegisterDependencyInjection();

            // Requires for only mvc areas
            // AreaRegistration.RegisterAllAreas();

            // Register web api configuration and its related routes
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Register error attribute which i believe gets overriden by the elmah
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Register Mvc routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfig.RegisterMappings();

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.UseCamelCase();
        }
    }

    public static class JsonMediaTypeFormatterExtention
    {
        public static void UseCamelCase(this JsonMediaTypeFormatter jsonMediaTypeFormatter)
        {
            var settings = jsonMediaTypeFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
