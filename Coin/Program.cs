using Microsoft.OpenApi.Models;
using System.Reflection;
using Refit;
using coin.src.Services.Refit;
using coin.src.Services.Interfaces;
using StackExchange.Redis;
using coin.src.Services;

namespace coin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");  

            builder.Services.AddSingleton<IConnectionMultiplexer>(redis);      

            builder.Services.AddSingleton<IQuotationService, QuotationService>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers();
            builder.Services.AddRefitClient<IEconomy>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(connectionString));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "client");
                });
            }
            app.UseRouting();
            app.MapControllers();

            var port = "9090";
            app.Run($"http://0.0.0.0:{port}");
        }
    }
}
