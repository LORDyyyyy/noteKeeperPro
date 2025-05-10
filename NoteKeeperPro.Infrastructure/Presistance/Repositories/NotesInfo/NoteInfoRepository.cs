using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteKeeperPro.Domain.Entities.NotesInfo;
using NoteKeeperPro.Infrastructure.Presistance.Data;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.NotesInfo
{
    public class NoteInfoRepository : INoteInfoRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public NoteInfoRepository(ApplicationDbContext dbContext) // Ask Clr To Create Instnce (Dependeny Injection)
        {
            _dbContext = dbContext;
        }

        public int AddNoteInfo(NoteInfo noteInfo)
        {
            _dbContext.NoteInfos.Add(noteInfo); // Saved Locally
            return _dbContext.SaveChanges(); // Apply Remotly
        }

        public int DeleteNoteInfo(NoteInfo noteInfo)
        {
            _dbContext.NoteInfos.Remove(noteInfo);
            return _dbContext.SaveChanges(); // Apply Remotly
        }

        public IEnumerable<NoteInfo> GetAll(bool AsNoTracking = true)
        {
            if (AsNoTracking)
            {
                // Detached
                return _dbContext.NoteInfos.AsNoTracking().ToList();
            }

            // Unchanged
            return _dbContext.NoteInfos.ToList();
        }

        public IQueryable<NoteInfo> GetAllQuarable(bool AsNoTracking = true)
        {
            return _dbContext.NoteInfos;
        }

        public NoteInfo? GetById(int id)
        {
            return _dbContext.NoteInfos.Find(id); // Search Localy , If Found Rteurn True , else send => Request Database 
        }

        public int UpdateNoteInfo(NoteInfo noteInfo)
        {
            _dbContext.NoteInfos.Update(noteInfo); // Saved Locally
            return _dbContext.SaveChanges(); // Apply Remotly
        }
    }
}
