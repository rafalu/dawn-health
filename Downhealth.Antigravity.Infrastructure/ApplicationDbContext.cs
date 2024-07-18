using Dawnhealth.Antigravity.Domain;
using Dawnhealth.Antigravity.Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace Downhealth.Antigravity.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<ApplicationUser> Users { get; set; }

    public DbSet<ActivationCode> ActivationCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=demo.db");
    }
}
