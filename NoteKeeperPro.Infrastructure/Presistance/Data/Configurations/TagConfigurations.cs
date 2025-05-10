using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteKeeperPro.Domain.Entities.Tags;

namespace NoteKeeperPro.Infrastructure.Presistance.Data.Configurations
{
    public class TagConfigurations : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired().HasMaxLength(255); 

        }
    }
}
