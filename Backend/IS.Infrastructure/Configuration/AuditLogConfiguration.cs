using IS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IS.Infrastructure.Configuration
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.TableName).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(a => a.RecordId).IsRequired();
            builder.Property(a => a.FieldName).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(a => a.OldValue).HasColumnType("nvarchar(300)");
            builder.Property(a => a.NewValue).HasColumnType("nvarchar(300)");
            builder.Property(a => a.Action).HasColumnType("nvarchar(20)").IsRequired();
            builder.Property(a => a.ChangedAt).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");

            builder.HasIndex(a => new { a.TableName, a.RecordId });
        }
    }
}