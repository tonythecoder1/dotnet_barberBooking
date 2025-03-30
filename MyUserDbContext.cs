using dotidentity;
using dotidentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class MyUserDbContext : IdentityDbContext<MyUser>  //bContext herda de IdentityDbContext<MyUser>
{
    public MyUserDbContext(DbContextOptions<MyUserDbContext> options)
        : base(options)
    {
    }

    public MyUserDbContext() {}

    public DbSet<Reserva> Reservas { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                "server=localhost;database=dotnet_db;user=root;password=12345678",
                ServerVersion.AutoDetect("server=localhost;database=dotnet_db;user=root;password=12345678")
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Organization>(org =>
    {
        org.ToTable("Organizations");          
        org.HasKey(o => o.Id);

        org.HasMany<MyUser>()
            .WithOne(u => u.Organization)
            .HasForeignKey(o => o.OrgId)
            .IsRequired(false);
    });

    modelBuilder.Entity<Reserva>(reserva =>
    {
        reserva.ToTable("Reservas");

        reserva.HasKey(r => r.Id);

        reserva.HasOne(r => r.Usuario)
            .WithMany(u => u.Reservas)
            .HasForeignKey(r => r.UserId)
            .IsRequired(false);
    });
}

}
