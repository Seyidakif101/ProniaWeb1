using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProniaWebSeyid.Configurations.ModelsConfiguration
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.Property(pi => pi.ImageUrl).IsRequired().HasMaxLength(500);
            builder.Property(pi => pi.ImageUrl).IsRequired();
            builder.HasOne(pi => pi.Product).WithMany(p => p.ProductImages).HasForeignKey(pi => pi.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
