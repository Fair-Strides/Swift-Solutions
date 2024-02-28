using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PopNGo.Areas.Identity.Data;

namespace PopNGo.Data;

public class ApplicationDbContext : IdentityDbContext<PopNGoUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     if (!optionsBuilder.IsConfigured)
    //     {
    //         optionsBuilder.UseSqlServer("Name=AppConnection");
    //     }
    //     optionsBuilder.UseLazyLoadingProxies();
    // }
}
