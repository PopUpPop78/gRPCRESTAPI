using gRPCRESTAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace gRPCRESTAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {
            
        }

        public DbSet<Item> Items => Set<Item>();
    }
}