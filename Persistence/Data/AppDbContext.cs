using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public required DbSet<Activity> Activities { get; set; }
    }
}
