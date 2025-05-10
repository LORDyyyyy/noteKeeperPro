using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Application.Dtos.Tags;

namespace NoteKeeperPro.Application.Services.Tags
{
    public interface ITagService
    {
        IEnumerable<TagToReturnDto> GetAllTags();
        TagDetailsToReturnDto? GetTagById(int id);
        int CreateTag(TagToCreateDto tag);
        int UpdateTag(TagToUpdateDto tag);
        bool DeleteTag(int id);
    }
}
