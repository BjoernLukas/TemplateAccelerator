using Microsoft.EntityFrameworkCore;

namespace TemplateAcceleratorV1.Models
{

    public class TemplateDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
        {
        }
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<Item> Items { get; set; }
    }

}
