using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteKeeperPro.Domain.Entities.NotesInfo;

namespace NoteKeeperPro.Infrastructure.Presistance.Data.Configurations
{
    internal class NoteInfoConfigrations : IEntityTypeConfiguration<NoteInfo>
    {
       
        public void Configure(EntityTypeBuilder<NoteInfo> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.CreatedAt).IsRequired() 
                .HasDefaultValueSql("GETDATE()"); 

            builder.Property(n => n.LastModifiedAt)
                .IsRequired()  
                .HasDefaultValueSql("GETDATE()"); 

            builder.Property(n => n.WordCount).IsRequired();  

            builder.Property(n => n.CharchterCount).IsRequired();  
        }
    }
}
