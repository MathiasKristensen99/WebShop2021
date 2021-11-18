using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using EASV.Webshop2021.Core.IServices;
using EASV.WebShop2021.DB;
using EASV.WebShop2021.DB.Entities;
using EASV.WebShop2021.DB.Repositories;
using EASV.Webshop2021.Domain.IRepositories;
using EASV.Webshop2021.Domain.Services;
using EASV.WebShop2021.Security;
using EASV.WebShop2021.Security.Repositories;
using EASV.WebShop2021.Security.Services;
using EASV.Webshop2021.WebApi.PolicyHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<LoginUserRepository>();
            services.AddScoped<IUserAuthenticatorService, UserAuthenticatorService>();
            services.AddScoped<IAuthService, AuthService>();
            
            var loggerFactory = LoggerFactory.Create(builder => {
                    builder.AddConsole();
                }
            );
            
            Byte[] secretBytes = new byte[40];

            using (var rngCsp = new System.Security.Cryptography.RNGCryptoServiceProvider() {})
            {
                rngCsp.GetBytes(secretBytes);
            }

            services.AddDbContext<WebShopDbContext>(
                opt =>
                {
                    opt
                        .UseLoggerFactory(loggerFactory)
                        .UseSqlite("Data Source=WebShopApp.db");
                }, ServiceLifetime.Transient);
            
            services.AddDbContext<AuthDbContext>(opt =>
            {
                opt.UseLoggerFactory(loggerFactory).
                UseSqlite("Data Source=auth.db"); 
            }, ServiceLifetime.Transient);
            
            services.AddTransient<ISecurityInitializer, SecurityInitializer>();
            
            
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    //IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])) //Configuration["JwtToken:SecretKey"]
                };
            });

            services.AddSingleton<IAuthorizationHandler, CanWriteProductsHandler>();
            services.AddSingleton<IAuthorizationHandler, CanReadProductsHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(CanWriteProductsHandler), 
                    policy => policy.Requirements.Add(new CanWriteProductsHandler()));
                options.AddPolicy(nameof(CanReadProductsHandler), 
                    policy => policy.Requirements.Add(new CanReadProductsHandler()));
            });
            
            services.AddCors(options => options
                .AddPolicy("dev-policy", policy =>
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EASV.Webshop2021.WebApi v1"));
                
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var ctx = services.GetService<WebShopDbContext>();
                    var securityCtx = services.GetService<AuthDbContext>();
                    var dbSecurityInit = services.GetService<ISecurityInitializer>();
                    dbSecurityInit.Initialize(securityCtx);

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
                    
                    app.UseCors("dev-policy");
                }
            }
            else
            {
                var scope = app.ApplicationServices.CreateScope();
                var services = scope.ServiceProvider;
                var ctx = services.GetService<WebShopDbContext>();
                var securityCtx = services.GetService<AuthDbContext>();
                var dbSecurityInit = services.GetService<ISecurityInitializer>();
                dbSecurityInit.Initialize(securityCtx);
                ctx.Database.EnsureDeleted();
                new DbSeeder(ctx).SeedProduction();
                ctx.Products.AddRange(new List<ProductEntity>
                {
                    new ProductEntity{Name = "Stor fed idiot"},
                    new ProductEntity{Name = "Forbandet lort"},
                    new ProductEntity{Name = "Pis og lort"}
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