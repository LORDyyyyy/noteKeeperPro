using System;
using System.Collections.Generic;
using System.Linq;
using Azure;
using NoteKeeperPro.Application.Dtos.Tags;
using NoteKeeperPro.Domain.Entities.Tags;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.Tags;

namespace NoteKeeperPro.Application.Services.Tags
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IEnumerable<TagToReturnDto> GetAllTags()
        {
            var tags = _tagRepository.GetAllQuarable()
                .Where(t => t.IsDeleted == false)
                .Select(t => new TagToReturnDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    NoteIds = t.NoteIds,
                }).ToList();

            return tags;
        }

        public TagDetailsToReturnDto? GetTagById(int id)
        {
            var tag = _tagRepository.GetById(id);

            if (tag != null)
            {
                return new TagDetailsToReturnDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    NoteIds = tag.NoteIds,

                };
            }

            return null;
        }

        public int CreateTag(TagToCreateDto tag)
        {
            var newTag = new Tag
            {
                Name = tag.Name,
                NoteIds = tag.NoteIds.ToList(),

            };

            return _tagRepository.AddTag(newTag);
        }

        public int UpdateTag(TagToUpdateDto tag)
        {
            var updatedTag = new Tag
            {
                Id = tag.Id,
                Name = tag.Name,
                NoteIds = tag.NoteIds.ToList(),
               
            };

            return _tagRepository.UpdateTag(updatedTag);
        }

        public bool DeleteTag(int id)
        {
            var tag = _tagRepository.GetById(id);

            if (tag != null)
            {
                return _tagRepository.DeleteTag(tag) > 0;
            }

            return false;
        }
    }
}
