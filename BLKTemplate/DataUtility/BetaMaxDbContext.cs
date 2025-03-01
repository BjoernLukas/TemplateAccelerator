using BetaMaxRMS.BetaMaxModels;
using Microsoft.EntityFrameworkCore;

namespace BetaMaxRMS.DataUtility;


public class BetaMaxDbContext : DbContext
{
    public BetaMaxDbContext(DbContextOptions<BetaMaxDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customer { get; set; }
    public DbSet<Movie> Movie { get; set; }

    public DbSet<MovieRental> MovieRental { get; set; }
}
