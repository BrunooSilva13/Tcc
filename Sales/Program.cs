using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesAPI.Models;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices((context, services) =>
                {
                    services.AddControllers();

                    // Adicionar HttpClient
                    services.AddHttpClient();

                    // Configurar string de conexão
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                    // Registrar SaleRepository com a string de conexão
                    services.AddTransient<ISaleRepository>(provider => new SaleRepository(connectionString));
                });
                webBuilder.Configure((context, app) =>
                {
                    var env = context.HostingEnvironment;
                    if (env.IsDevelopment())
                    {
                        app.UseDeveloperExceptionPage();
                    }
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            });
}
