using BetaMaxRMS.DataUtility;
using BetaMaxRMS.Services;
using Microsoft.EntityFrameworkCore;

namespace BetaMaxRMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register the BetaMaxDbContext with dependency injection  
            builder.Services.AddDbContext<BetaMaxDbContext>(options =>
            {
                options
                .UseSqlServer(builder.Configuration["ConnectionStrings:BLKDbContextConnection"]);
            });

            builder.Services.AddScoped<IMovieRentalCalculationService, MovieRentalCalculationService>();

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
