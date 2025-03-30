using dotidentity;
using dotidentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class MyUserDbContext : IdentityDbContext<MyUser>
{
    public MyUserDbContext(DbContextOptions<MyUserDbContext> options) : base(options) {}
    public MyUserDbContext() {}

    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Barbeiro> Barbeiros { get; set; }
    public DbSet<Servico> Servicos { get; set; }

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

        // Relacionamento com Organization
        modelBuilder.Entity<Organization>(org =>
        {
            org.ToTable("Organizations");
            org.HasKey(o => o.Id);
            org.HasMany<MyUser>()
                .WithOne(u => u.Organization)
                .HasForeignKey(o => o.OrgId)
                .IsRequired(false);
        });

        // Relacionamento com Barbeiro
        modelBuilder.Entity<Barbeiro>(barbeiro =>
        {
            barbeiro.ToTable("Barbeiros");
            barbeiro.HasKey(b => b.Id);
            barbeiro.HasMany(b => b.Reservas)
                .WithOne(r => r.Barbeiro)
                .HasForeignKey(r => r.BarbeiroId);
        });

        // Relacionamento com TipoDeCorte
        modelBuilder.Entity<Servico>(tipo =>
        {
            tipo.ToTable("Servicos");
            tipo.HasKey(t => t.Id);
            tipo.HasMany(t => t.Reservas)
                .WithOne(r => r.servico)
                .HasForeignKey(r => r.ServicoId);
        });

        // Relacionamento com Reserva
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
