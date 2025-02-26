using Microsoft.EntityFrameworkCore;
using TemplateAcceleratorV1.Models;

namespace TemplateAcceleratorV1.DataUtility;


public class TemplateDbContext : DbContext
{
    public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Item> Items { get; set; }
}
