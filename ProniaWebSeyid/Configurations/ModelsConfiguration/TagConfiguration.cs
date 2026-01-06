using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProniaWebSeyid.Configurations.ModelsConfiguration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(t => t.Name).IsRequired().HasMaxLength(30);
            builder.ToTable(x => x.HasCheckConstraint("CK_Tag_Name", "LEN([Name])>2"));
            builder.HasMany(t => t.ProductTags).WithOne(p => p.Tag).HasForeignKey(p => p.TagId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
