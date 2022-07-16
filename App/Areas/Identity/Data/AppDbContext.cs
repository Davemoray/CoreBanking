using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Claims>? Claims { get; set; }
    public DbSet<ApplicationRole>? ApplicationRole { get; set; }
    public DbSet<GLCategory>? GLCategory { get; set; }
    public DbSet<GLAccount>? GLAccount { get; set; }
    public DbSet<Customer>? Customer { get; set; }
    public DbSet<AccountConfiguration>? AccountConfiguration { get; set; }
    public DbSet<Branch>? Branch { get; set; }
    public DbSet<TillUser>? TillUser { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }



    public DbSet<App.Models.CustomerAccount>? CustomerAccount { get; set; }



}
