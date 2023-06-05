using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TracNghiem.Domain;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Application.Behavior;
using TracNghiem.WebAPI.Application.Commands.Categories;
using TracNghiem.WebAPI.Application.Validations;
using TracNghiem.WebAPI.Infrastructure.Filters;
using TracNghiem.WebAPI.Infrastructure.Queries;
using TracNghiem.WebAPI.Services;
using TracNghiem.WebAPI.SignalR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200", "https://testkhanh.ddns.net", "*", "http://192.168.1.2", "http://TracNghiem.ddns.net:4200", "http://116.110.199.1:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed(origin => true) // allow any origin
                            .AllowCredentials(); // allow credentials
                    });
            });
            /*//WithOrigins("http://localhost:4200",
            //"http://trieunguyen.epizy.com/")*/
            services.AddSignalR();
            services.Configure<Audience>(Configuration.GetSection("Audience"));
            services.AddControllers(options =>
            {
                // bổ sung filter khi có ngoại lệ xảy ra
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TracNghiem.WebAPI", Version = "v1" });
                //add frame to get token bearer
                c.AddSecurityDefinition("Bearer",
                   new OpenApiSecurityScheme
                   {
                       In = ParameterLocation.Header,
                       Description = "Please enter into field the word 'Bearer' following by space and JWT",
                       Name = "Authorization",
                       Type = SecuritySchemeType.ApiKey,
                       Scheme = "MyAuthKey"
                   });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            services.AddDbContext<TracnghiemContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"]);
            }, ServiceLifetime.Scoped);
            //set up dependency entity
            //set up DI Repository
            //services.AddTransient<IRepository<Entity>, Repository<Entity>>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IExamRepository, ExamRepository>();
            services.AddTransient<ILevelRepository, LevelRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<ITestRepository, TestRepository>();
            services.AddTransient<IUserRepository, UserRepository>();


            services.AddTransient<IIdentityServices, IdentityServices>();
            services.AddTransient<IAppQueries>(x => new AppQueries(Configuration["ConnectionString"]));

            //set up mediator
            services.AddMediatR(Assembly.GetExecutingAssembly());
            // Assembly để .net vào bin và get file dll của những command DI với nhau không cần khai báo tất cả
            services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(AddCategoryCommand).Assembly);
            //chỉ lấy những file dll like addcategoryCommand

            //setup validator for fluent validator
            AssemblyScanner
                .FindValidatorsInAssembly(typeof(AddCategoryCommandValidator).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));


            var audience = Configuration.GetSection("Audience").Get<Audience>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audience.Secret));
            var tokenValidationParameters = new TokenValidationParameters //verify token
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audience.Issuer,
                ValidateAudience = true,
                ValidAudience = audience.Name,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            services.AddAuthentication()
                .AddJwtBearer("MyAuthKey", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TracNghiem.WebAPI v1"));
                /*app.UseSwaggerUI(c=> {
                    c.DisplayRequestDuration();
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TracNghiem.WebAPI v1");
                });*/
            }

            app.UseRouting();

            app.UseCors("CorsPolicy"); //applie doamin allow config above cross origin request

            app.UseAuthorization();
            app.UseAuthentication();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // .RequireCors(MyAllowSpecificOrigins);
                endpoints.MapHub<TracnghiemHub>("/signalr");
            });
        }
    }
}
