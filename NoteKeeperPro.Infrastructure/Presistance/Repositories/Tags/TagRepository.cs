using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteKeeperPro.Domain.Entities.Tags;
using NoteKeeperPro.Infrastructure.Presistance.Data;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Tag> GetAll(bool AsNoTracking = true)
        {
            var query = _context.Tags.AsQueryable();
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query.ToList();
        }

        public IQueryable<Tag> GetAllQuarable(bool AsNoTracking = true)
        {
            var query = _context.Tags.AsQueryable();
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query;
        }

        public Tag GetById(int id)
        {
            return _context.Tags.Find(id);
        }

        public int AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
            return _context.SaveChanges();
        }

        public int UpdateTag(Tag tag)
        {
            _context.Tags.Update(tag);
            return _context.SaveChanges();
        }

        public int DeleteTag(Tag tag)
        {
            _context.Tags.Remove(tag);
            return _context.SaveChanges();
        }

        public async Task<Tag> GetOrCreateTagAsync(string name)
        {
            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == name);

            if (tag == null)
            {
                tag = new Tag { Name = name };
                await _context.Tags.AddAsync(tag);
                await _context.SaveChangesAsync();
            }

            return tag;
        }

        public async Task<Tag> GetByNameAsync(string name)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
