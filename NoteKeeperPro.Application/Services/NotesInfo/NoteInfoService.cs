using System;
using System.Collections.Generic;
using System.Linq;
using NoteKeeperPro.Application.Dtos.NotesInfo;
using NoteKeeperPro.Domain.Entities.NotesInfo;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.NotesInfo;

namespace NoteKeeperPro.Application.Services.NotesInfo
{
    public class NoteInfoService : INoteInfoService
    {
        private readonly INoteInfoRepository _noteInfoRepository;

        public NoteInfoService(INoteInfoRepository noteInfoRepository)
        {
            _noteInfoRepository = noteInfoRepository;
        }

        public IEnumerable<NoteInfoToReturnDto> GetAllNoteInfos()
        {
            var notesInfo = _noteInfoRepository.GetAllQuarable()
                .Where(n => n.IsDeleted == false)
                .Select(n => new NoteInfoToReturnDto
                {
                    Id = n.Id,
                    CreatedAt = n.CreatedAt,
                    LastModifiedAt = n.LastModifiedAt ,
                    WordCount = n.WordCount,
                    CharchterCount = n.CharchterCount,
                })
                .ToList();

            return notesInfo;
        }

        public NoteInfoDetailsToReturnDto? GetNoteInfoById(int id)
        {
            var noteInfo = _noteInfoRepository.GetById(id);

            if (noteInfo != null)
            {
                return new NoteInfoDetailsToReturnDto
                {
                    Id = noteInfo.Id,
                    LastModifiedAt = noteInfo.LastModifiedAt,
                    CreatedAt = noteInfo.CreatedAt,
                    WordCount = noteInfo.WordCount,
                    CharchterCount = noteInfo.CharchterCount,
                };
            }

            return null;
        }

        public int CreateNoteInfo(NoteInfoToCreateDto noteInfo)
        {
            var newNoteInfo = new NoteInfo
            {
                LastModifiedAt = noteInfo.LastModifiedAt,
                CreatedAt = noteInfo.CreatedAt,
                WordCount = noteInfo.WordCount,
                CharchterCount = noteInfo.CharchterCount,
            };

            return _noteInfoRepository.AddNoteInfo(newNoteInfo);
        }

        public int UpdateNoteInfo(NoteInfoToUpdateDto noteInfo)
        {
            var updatedNoteInfo = new NoteInfo
            {
                Id = noteInfo.Id,
                LastModifiedAt = noteInfo.LastModifiedAt,
                CreatedAt = noteInfo.CreatedAt,
                WordCount = noteInfo.WordCount,
                CharchterCount = noteInfo.CharchterCount,
            };

            return _noteInfoRepository.UpdateNoteInfo(updatedNoteInfo);
        }

        public bool DeleteNoteInfo(int id)
        {
            var noteInfo = _noteInfoRepository.GetById(id);

            if (noteInfo != null)
            {
                return _noteInfoRepository.DeleteNoteInfo(noteInfo) > 0;
            }

            return false;
        }
    }
}
