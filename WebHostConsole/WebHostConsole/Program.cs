using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebHostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                 .UseContentRoot(Directory.GetCurrentDirectory())
                 .UseIISIntegration()
                 .UseStartup<Startup>()
                 .UseHttpSys(options =>
                 {
                     options.Authentication.Schemes = AuthenticationSchemes.None;
                     options.Authentication.AllowAnonymous = true;
                     options.MaxConnections = 100;
                     options.UrlPrefixes.Add("http://+:32767/");
                 })
                 .Build();

            host.Run();
        }
    }
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //entityframework
            //services.AddDbContext<todoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

            services.AddCors();

            services.AddMvc(c => {

                #region 依照Namespace而決定是否顯示
                //c.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
                #endregion
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Test API",
                    Version = "v1"
                });
                c.SwaggerDoc("v2", new Info
                {
                    Title = "Test API2",
                    Version = "v2"
                });

                #region 特定條件顯示
                //c.DocInclusionPredicate((docName, apiDesc) =>
                //{
                //    var versions = apiDesc.ControllerAttributes()
                //        .OfType<ApiExplorerSettingsAttribute>()
                //        .SelectMany(attr => attr.GroupName);

                //    return versions.Any(v => $"v{v.ToString()}" == docName);

                //    if (apiDesc.RelativePath.Split('/').Contains(docName))
                //    {
                //        string[] t1 = { "test", "a" };
                //        apiDesc.RelativePath.Split('/').Where(a => t1.Contains(a));
                //        return true;
                //    }
                //    else
                //        return false;
                //});
                #endregion

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "WebHostConsole.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(builder =>
             builder.AllowAnyOrigin()
            );

            //app.UseSwagger(c=>{
            //    c.RouteTemplate = "swagger/{documentName}";
            //});
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Test API V2");
            });

            app.UseMvc(route =>
            {
                // SwaggerGen won't find controllers that are routed via this technique.
                route.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}"
                    );
            });

        }
    }
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace; // e.g. "Controllers.V1"
            var apiVersion = controllerNamespace.Split('.').Last().ToLower();

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }

}
