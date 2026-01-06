using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProniaWebSeyid.Configurations.ModelsConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.ToTable(x => x.HasCheckConstraint("CK_Product_Name", "LEN([Name])>3"));
            builder.Property(p => p.Description).IsRequired(false).HasMaxLength(50);
            builder.Property(p => p.Price).HasPrecision(18, 2).IsRequired();
            builder.ToTable(x => x.HasCheckConstraint("CK_Product_Price", "Price>=0"));
            builder.Property(p => p.CategoryId).IsRequired();
            builder.Property(p => p.ReytingCount).IsRequired();
            builder.Property(p => p.ReytingCount).IsRequired();
            builder.ToTable(x => x.HasCheckConstraint("CK_Product_ReytingCount","ReytingCount >= 0 AND ReytingCount <= 5"));
            builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.ProductImages).WithOne(p => p.Product).HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.ProductTags).WithOne(pt => pt.Product).HasForeignKey(pt => pt.ProductId).OnDelete(DeleteBehavior.Cascade);


        }
    }
}
