using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.NotesInfo;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.NotesInfo
{
    public interface INoteInfoRepository
    {
        IEnumerable<NoteInfo> GetAll(bool AsNoTracking = true);
        IQueryable<NoteInfo> GetAllQuarable(bool AsNoTracking = true);
        NoteInfo GetById(int id);
        int AddNoteInfo(NoteInfo noteInfo);
        int UpdateNoteInfo(NoteInfo noteInfo);
        int DeleteNoteInfo(NoteInfo noteInfo);
    }
}
