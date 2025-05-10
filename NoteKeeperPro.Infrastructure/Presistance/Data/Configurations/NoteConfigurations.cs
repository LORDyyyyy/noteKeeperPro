using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Infrastructure.Presistance.Data.Configurations
{
    public class NoteConfigurations : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title).IsRequired().HasMaxLength(100);

            builder.Property(n => n.Content).IsRequired().HasColumnType("nvarchar(max)");


         
    }

    }
}
