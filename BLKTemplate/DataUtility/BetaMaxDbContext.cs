using BetaMaxRMS.BetaMaxModels;
using Microsoft.EntityFrameworkCore;

namespace BetaMaxRMS.DataUtility;


public class BetaMaxDbContext : DbContext
{
    public BetaMaxDbContext(DbContextOptions<BetaMaxDbContext> options) : base(options)
    {
    }

    public DbSet<BetaMaxCustomer> Persons { get; set; }
    public DbSet<Movie> Items { get; set; }
}
