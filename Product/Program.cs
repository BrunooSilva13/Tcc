using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Product.src.Repository;
using Product.src.Service;
using Product.src.Repositories.Interface;
using Product.src.Repository.Interface;
using Microsoft.OpenApi.Models;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = builder.Configuration;
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddScoped<IProductRepository, ProductRepository>(provider =>
            new ProductRepository(connectionString));
        builder.Services.AddScoped<ProductService>();

        builder.Services.AddControllers();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Product", Version = "v1" });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nome do seu Projeto v1");
            });
        }

        app.UseRouting();
        app.MapControllers();
        app.Run();
    }
}
