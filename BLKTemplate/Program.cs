using Microsoft.EntityFrameworkCore;
using NSwag;
using TemplateAcceleratorV1.DataUtility;

namespace TemplateAcceleratorV1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);            

            builder.Services.AddControllers();            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApiDocument();

            // Register the TemplateDbContext with dependency injection  
            builder.Services.AddDbContext<TemplateDbContext>(options => { options
                .UseSqlServer(builder.Configuration["ConnectionStrings:BLKDbContextConnection"]); });            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

           

            app.Run();
        }
    }
}
