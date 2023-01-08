using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProiectPSSC.Data;
using ProiectPSSC.Data.Repositories;
using ProiectPSSC.Domain;
using ProiectPSSC.Domain.Repositories;

namespace Proiect.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OrderContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("SQL1")));
          
            services.AddTransient<IOrderLineRepository, OrderLineRepository>();
            services.AddTransient<IOrderHeaderRepository, OrderHeaderRepository>();
            services.AddTransient<IProductRepository,ProductRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<PlaceOrderWorkflow>();

            services.AddHttpClient();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1001", new OpenApiInfo { Title = "ProiectPSSC.Api", Version = "v1001" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1001/swagger.json", "ProiectPSSC.Api v1001"));
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
