using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using firstApi.Core.Interfaces;
using firstApi.Core.Services;
using firstApi.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
//using Newtonsoft.Json.Serialization;

namespace firstApi.Api
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            /*
                .AddJsonOptions(o =>
            {
                if (o.SerializerSettings.ContractResolver != null)
                {
                    var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                    castedResolver.NamingStrategy = null;  // uses in JSON names of properties in our models
                }
            }); */
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else 
            services.AddTransient<IMailService, CloudMailService>();
#endif
            var connectionString = Startup.Configuration["connectionString:personInfoDBConnectionString"];
            services.AddDbContext<PersonInfoContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IPersonRepository, PersonRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, PersonInfoContext personInfoContext)
        {
            //loggerFactory.AddConsole();
            //loggerFactory.AddDebug();
            // loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            personInfoContext.EnsureSeedDataForContext();
            app.UseMvc();
            app.UseStatusCodePages();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
