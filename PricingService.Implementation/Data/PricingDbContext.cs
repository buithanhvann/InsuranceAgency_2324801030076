using Microsoft.EntityFrameworkCore;
using PricingService.Domain;

namespace PricingService.Implementation.Data;

public class PricingDbContext : DbContext
{
    public PricingDbContext(DbContextOptions<PricingDbContext> options) : base(options) { }

    public DbSet<TariffEntity> Tariffs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TariffEntity>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.BasePremiumRulesJson).IsRequired();
            entity.Property(e => e.DiscountMarkupRulesJson).IsRequired();
        });
    }
}

public class TariffEntity
{
    public string Code { get; set; } = string.Empty;
    public string BasePremiumRulesJson { get; set; } = string.Empty;
    public string DiscountMarkupRulesJson { get; set; } = string.Empty;
}