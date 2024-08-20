
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using MessagingService.Data;
using MessagingService.Mapper;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.Messenger
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
            builder.Services.AddSwaggerGen();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(cb =>
            {
                var connectionString = builder.Configuration
                                              .GetConnectionString("db")
                                              ?? throw new NullReferenceException("Connection string can't be Null");

                cb.Register(c =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<MessageContext>();
                    optionsBuilder.UseMySQL(connectionString)
                                  .UseLazyLoadingProxies();
                    return new MessageContext(optionsBuilder.Options);
                }).InstancePerDependency();

                cb.Register(ctx =>
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<MappingProfile>();
                    });

                    return config.CreateMapper();
                }).As<IMapper>().SingleInstance();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
