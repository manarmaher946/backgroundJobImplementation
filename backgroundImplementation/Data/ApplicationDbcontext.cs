using backgroundImplementation.Modeals;
using Microsoft.EntityFrameworkCore;

namespace backgroundImplementation.Data
{
    public class ApplicationDbcontext : DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        {
        }
       public DbSet<Product>products { get; set; } 
    }
}
