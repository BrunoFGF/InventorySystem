using IS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IS.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(u => u.LastName).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(u => u.Phone).HasColumnType("nvarchar(10)").IsRequired();
            builder.Property(u => u.Email).HasColumnType("nvarchar(255)").IsRequired();
            builder.Property(u => u.PasswordHash).HasColumnType("nvarchar(255)").IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}