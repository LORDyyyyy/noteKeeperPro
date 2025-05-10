using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Application.Dtos.NotesInfo;

namespace NoteKeeperPro.Application.Services.NotesInfo
{
    public interface INoteInfoService
    {
        IEnumerable<NoteInfoToReturnDto> GetAllNoteInfos();
        NoteInfoDetailsToReturnDto? GetNoteInfoById(int id);
        int CreateNoteInfo(NoteInfoToCreateDto noteInfo);
        int UpdateNoteInfo(NoteInfoToUpdateDto noteInfo);
        bool DeleteNoteInfo(int id);
    }
}
