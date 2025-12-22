using Microsoft.EntityFrameworkCore;
using ProniaWebSeyid.Models;

namespace ProniaWebSeyid.Contexts
{
    public class AddDbContext:DbContext
    {
        public AddDbContext(DbContextOptions<AddDbContext> options) : base(options)
        {
        }
        public DbSet<Shipping> Shippings { get; set; }
    }
}
