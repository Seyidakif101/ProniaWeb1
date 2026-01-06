using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProniaWebSeyid.Configurations.ModelsConfiguration
{
    public class ShippingConfiguration : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> builder)
        {
            builder.Property(s => s.Name).IsRequired().HasMaxLength(50);
            builder.ToTable(x => x.HasCheckConstraint("CK_Shipping_Name", "LEN([Name])>3"));
            builder.Property(s => s.Description).IsRequired(false).HasMaxLength(200);
        }
    }
}
