using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Client.Repositories;
using Client.Services;
using Client.src.Repositories.Interface;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // IConfiguration configuration = builder.Configuration;
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection");

    
        builder.Services.AddScoped<IClientRepository, ClientRepository>(provider =>
            new ClientRepository(connectionString));
        builder.Services.AddScoped<ClientService>();

        builder.Services.AddControllers();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Client", Version = "v1" });
        });

        builder.Services.AddLogging(logging =>
       {
           logging.ClearProviders(); // Limpa os provedores de log padrão

           // Configura o logging com base nas configurações em appsettings.json
           logging.AddConfiguration(configuration.GetSection("Logging"));

           // Adiciona logging para o console
           logging.AddConsole();

           // Adiciona logging para o Debug
           logging.AddDebug();
       });


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tcc");
            });
        }

        app.UseRouting();
        app.MapControllers();
        app.Run();
    }

}
