using IS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IS.Infrastructure.Configuration
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(s => s.Email).HasColumnType("nvarchar(255)");
            builder.Property(s => s.Phone).HasColumnType("nvarchar(10)");
        }
    }
}