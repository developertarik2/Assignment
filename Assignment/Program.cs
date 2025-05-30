
using Assignment.Application.Models.Dtos;
using Assignment.Application.Validators;
using Assignment.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Assignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var config = builder.Configuration;
                var redisHost = config["Redis:Host"];
                var redisPort = config["Redis:Port"];
                return ConnectionMultiplexer.Connect($"{redisHost}:{redisPort}");
            });

            builder.Services.AddDbContext<FinDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection")));

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.Configure<SmsSettings>(builder.Configuration.GetSection("SmsSettings"));

            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
