using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProniaWebSeyid.Configurations.ModelsConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {

        public void Configure(EntityTypeBuilder<Category> builder)
        {
           builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        }
    }
}
