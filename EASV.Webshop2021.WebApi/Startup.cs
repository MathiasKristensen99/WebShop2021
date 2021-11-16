using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EASV.Webshop2021.Core.IServices;
using EASV.WebShop2021.DB;
using EASV.WebShop2021.DB.Entities;
using EASV.WebShop2021.DB.Repositories;
using EASV.Webshop2021.Domain.IRepositories;
using EASV.Webshop2021.Domain.Services;
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

namespace EASV.Webshop2021.WebApi
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "EASV.Webshop2021.WebApi", Version = "v1"});
            });

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            
            var loggerFactory = LoggerFactory.Create(builder => {
                    builder.AddConsole();
                }
            );
            
            services.AddDbContext<WebShopDbContext>(
                opt =>
                {
                    opt
                        .UseLoggerFactory(loggerFactory)
                        .UseSqlite("Data Source=WebShopApp.db");
                }, ServiceLifetime.Transient);

            services.AddCors(options => options
                .AddPolicy("dev-policy", policy =>
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebShopDbContext ctx)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EASV.Webshop2021.WebApi v1"));
                
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    //var services = scope.ServiceProvider;
                    //var ctx = services.GetService<WebShopDbContext>();
                    ctx.Database.EnsureDeleted();
                    ctx.Database.EnsureCreated();
                    ctx.Products.AddRange(new List<ProductEntity>
                    {
                        new ProductEntity{Name = "Product1"},
                        new ProductEntity{Name = "Product2"},
                        new ProductEntity{Name = "Product3"}
                    });
                    ctx.SaveChanges();
                }
            }
            else
            {
                new DbSeeder(ctx).SeedProduction();   
                ctx.Products.AddRange(new List<ProductEntity>
                {
                    new ProductEntity{Name = "Product1"},
                    new ProductEntity{Name = "Product2"},
                    new ProductEntity{Name = "Product3"}
                });
                ctx.SaveChanges();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("dev-policy");

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}