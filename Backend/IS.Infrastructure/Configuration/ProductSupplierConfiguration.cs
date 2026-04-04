using IS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IS.Infrastructure.Configuration
{
    public class ProductSupplierConfiguration : IEntityTypeConfiguration<ProductSupplier>
    {
        public void Configure(EntityTypeBuilder<ProductSupplier> builder)
        {
            builder.ToTable("ProductSuppliers");
            builder.HasKey(ps => ps.Id);
            builder.Property(ps => ps.Price).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(ps => ps.Stock).IsRequired();
            builder.Property(ps => ps.BatchNumber).HasColumnType("nvarchar(50)");

            builder.HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ps => ps.Supplier)
                .WithMany(s => s.ProductSuppliers)
                .HasForeignKey(ps => ps.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ps => new { ps.ProductId, ps.SupplierId, ps.BatchNumber }).IsUnique();

            builder.HasQueryFilter(ps => !ps.Product.IsDeleted);
        }
    }
}