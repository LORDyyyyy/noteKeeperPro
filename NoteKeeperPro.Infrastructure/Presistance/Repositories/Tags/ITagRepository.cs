using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Tags;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.Tags
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAll(bool AsNoTracking = true);
        IQueryable<Tag> GetAllQuarable(bool AsNoTracking = true);
        Tag GetById(int id);
        int AddTag(Tag tag);
        int UpdateTag(Tag tag);
        int DeleteTag(Tag tag);
        Task<Tag> GetOrCreateTagAsync(string name);
        Task<Tag> GetByNameAsync(string name);
        Task<Tag> GetByIdAsync(int id);
    }
}
