using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Domain.Entities.Collaborators;
using NoteKeeperPro.Domain.Entities.M_M_RelationShips;
using NoteKeeperPro.Domain.Entities.Notes;
using NoteKeeperPro.Domain.Entities.NotesInfo;
using NoteKeeperPro.Domain.Entities.Tags;
using NoteKeeperPro.Infrastructure.Identity;

namespace NoteKeeperPro.Infrastructure.Presistance.Data
{
    // Repository => ApplicationDbContext (DataBase) 
    // ApplicationDbContext : IdentityUser

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // Doesnt Suitable For new Way of Dependency Injection
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=NoteKeeperPro;trusted_connection=true;trustServerCertificate=true;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating (modelBuilder); 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // Apply All Configurations Classes

            // علاقة Note مع ApplicationUser
            modelBuilder.Entity<Note>()
                .HasOne(n => n.Owner)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ تجنب الحذف التلقائي

            // علاقة Note مع NoteInfo (One-to-One)
            modelBuilder.Entity<Note>()
                .HasOne(n => n.NoteInfo)
                .WithOne(ni => ni.Note)
                .HasForeignKey<NoteInfo>(ni => ni.NoteId)
                .OnDelete(DeleteBehavior.Restrict); // ✅

            // علاقة NoteTag (Many-to-Many بين Note و Tag)
            modelBuilder.Entity<NoteTag>()
                .HasKey(nt => new { nt.NoteId, nt.TagId });

            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Note)
                .WithMany(n => n.NoteTags)
                .HasForeignKey(nt => nt.NoteId)
                .OnDelete(DeleteBehavior.Restrict); // ✅

            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NoteTags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Restrict); // ✅

            // علاقة NoteCollaborator (Many-to-Many بين Note و Collaborator)
            modelBuilder.Entity<NoteCollaborator>()
                .HasKey(nc => new { nc.NoteId, nc.CollaboratorId });

            modelBuilder.Entity<NoteCollaborator>()
                .HasOne(nc => nc.Note)
                .WithMany(n => n.NoteCollaborators)
                .HasForeignKey(nc => nc.NoteId)
                .OnDelete(DeleteBehavior.Restrict); // ✅

            modelBuilder.Entity<NoteCollaborator>()
                .HasOne(nc => nc.Collaborator)
                .WithMany(c => c.NoteCollaborators)
                .HasForeignKey(nc => nc.CollaboratorId)
                .OnDelete(DeleteBehavior.Restrict); // ✅
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteInfo> NoteInfos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Collaborator> Collaborators { get; set; }

        public DbSet<NoteTag> NoteTags { get; set; }
        public DbSet<NoteCollaborator> NoteCollaborators { get; set; }

    }
}
