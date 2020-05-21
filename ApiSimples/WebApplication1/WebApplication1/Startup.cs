    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication1.models;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //O m�todo ConfigureServices() � respons�vel por definir os servi�os que a aplica��o vai usar
        //IServiceCollection permite configurar diferentes tipos de servi�os
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TarefaContext>(opt => opt.UseInMemoryDatabase("Database"));
            services.AddScoped<TarefaContext, TarefaContext>(); 
            services.AddControllers();

            //1 -Transient : Criado a cada vez que s�o solicitados.
            //2 - Scoped: Criado uma vez por solicita��o.
            //3 - Singleton: Criado na primeira vez que s�o solicitados.Cada solicita��o subseq�ente usa a inst�ncia que foi criada na primeira vez.
            services.AddTransient<ITarefaRepositorio, TarefaRepositorio>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
