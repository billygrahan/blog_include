using bog_include.Models;
using Microsoft.EntityFrameworkCore;

namespace bog_include.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Postagem>? Postagems { get; set; }
    }
}
