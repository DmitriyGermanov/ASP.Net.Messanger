using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;
using UserService.Data;
using UserService.Mapper;
using UserService.Repo;
using UserService.rsa;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
        
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "Token",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
                });
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new RsaSecurityKey(RSATools.GetPublicKey())
                    };
                });

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                        .ConfigureContainer<ContainerBuilder>(cb =>
                        {
                            var connectionString = builder.Configuration
                                                          .GetConnectionString("db")
                                                          ?? throw new NullReferenceException("Connection string can't be Null");

                            cb.Register(c =>
                            {
                                var optionsBuilder = new DbContextOptionsBuilder<UserServiceContext>();
                                optionsBuilder.UseMySQL(connectionString)
                                              .UseLazyLoadingProxies();
                                return new UserServiceContext(optionsBuilder.Options);
                            }).InstancePerDependency();

                            cb.Register(ctx =>
                            {
                                var config = new MapperConfiguration(cfg =>
                                {
                                    cfg.AddProfile<MappingProfile>();
                                });

                                return config.CreateMapper();
                            }).As<IMapper>().SingleInstance();

                            cb.RegisterType<UserRepository>().As<IUserRepository>()
                                                                        .InstancePerLifetimeScope();

                        });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
