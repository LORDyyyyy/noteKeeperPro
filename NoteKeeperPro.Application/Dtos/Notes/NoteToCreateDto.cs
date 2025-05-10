using NoteKeeperPro.Application.Dtos.ApplicationsUsers;
using NoteKeeperPro.Application.Dtos.Collaborators;
using NoteKeeperPro.Application.Dtos.NotesInfo;
using NoteKeeperPro.Application.Dtos.Tags;

namespace NoteKeeperPro.Application.Dtos.Notes
{
    public class NoteToCreateDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }

        public string OwnerId { get; set; }

        public ApplicationUserDto Owner { get; set; }

        public NoteInfoDetailsToReturnDto NoteInfo { get; set; }

        public ICollection<CollaboratorDetailsToReturnDto> Collaborators { get; set; }

        public ICollection<TagDetailsToReturnDto> Tags { get; set; }


    }
}
