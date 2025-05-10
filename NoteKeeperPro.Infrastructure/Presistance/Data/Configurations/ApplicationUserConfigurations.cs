using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Domain.Entities.Tags;

namespace NoteKeeperPro.Infrastructure.Presistance.Data.Configurations
{
   
        internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
        {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // إعداد المفتاح الأساسي
            builder.HasKey(u => u.Id);

            // إعداد خصائص المستخدم (UserName, Email, PhoneNumber, etc.)
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.NormalizedUserName)
                .HasMaxLength(256);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

          
            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(15);

            builder.Property(u => u.LockoutEnd)
                .HasColumnType("datetime");

            builder.Property(u => u.ConcurrencyStamp)
                .IsConcurrencyToken();

            builder
    .Property(u => u.LockoutEnd)
    .HasConversion(
        v => v.HasValue ? v.Value.DateTime : (DateTime?)null,
        v => v.HasValue ? new DateTimeOffset(v.Value) : (DateTimeOffset?)null);

        }
    }
    }


