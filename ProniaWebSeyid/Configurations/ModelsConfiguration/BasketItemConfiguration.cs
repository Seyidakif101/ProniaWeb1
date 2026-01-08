using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace ProniaWebSeyid.Configurations.ModelsConfiguration;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.Property(b => b.Count).IsRequired();
        builder.ToTable(options =>
        {
            options.HasCheckConstraint("_CK_BasketItem_Count", "[Count]>0");
        });
        builder.HasIndex(b => new { b.ProductId, b.AppUserId }).IsUnique();
        builder.HasOne(b => b.Product).WithMany(p => p.BasketItems).HasForeignKey(b => b.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(b => b.AppUser).WithMany(au => au.BasketItems).HasForeignKey(b => b.AppUserId).OnDelete(DeleteBehavior.Cascade);

    }
}
