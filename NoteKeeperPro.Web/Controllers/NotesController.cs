using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteKeeperPro.Application.Dtos.Notes;
using NoteKeeperPro.Application.Services.Notes;

namespace NoteKeeperPro.Web.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var notes = await _noteService.GetAllNotesAsync(userId);
            return View(notes);
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                var note = await _noteService.GetNoteByIdAsync(id, userId);
                return View(note);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            return View(new CreateNoteDto());
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNoteDto createNoteDto)
        {
            if (!ModelState.IsValid)
                return View(createNoteDto);

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                var note = await _noteService.CreateNoteAsync(createNoteDto, userId);
                return RedirectToAction(nameof(Details), new { id = note.Id });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while creating the note.");
                return View(createNoteDto);
            }
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                var note = await _noteService.GetNoteByIdAsync(id, userId);
                var updateDto = new UpdateNoteDto
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    TagNames = note.Tags.Select(t => t.Name).ToList(),
                    CollaboratorEmails = note.Collaborators.Select(c => c.UserName).ToList()
                };
                return View(updateDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: Notes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateNoteDto updateNoteDto)
        {
            if (id != updateNoteDto.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(updateNoteDto);

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                var note = await _noteService.UpdateNoteAsync(updateNoteDto, userId);
                return RedirectToAction(nameof(Details), new { id = note.Id });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the note.");
                return View(updateNoteDto);
            }
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                var note = await _noteService.GetNoteByIdAsync(id, userId);
                return View(note);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var result = await _noteService.DeleteNoteAsync(id, userId);
            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: Notes/Share/5
        public async Task<IActionResult> Share(int id)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                var note = await _noteService.GetNoteByIdAsync(id, userId);
                ViewBag.NoteId = id;
                ViewBag.NoteTitle = note.Title;
                return View();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: Notes/Share/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Share(int id, string collaboratorEmail)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(collaboratorEmail))
            {
                ModelState.AddModelError("", "Collaborator email is required.");
                return View();
            }

            var result = await _noteService.ShareNoteAsync(id, collaboratorEmail, userId);
            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Notes/RemoveCollaborator/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCollaborator(int id, string collaboratorEmail)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var result = await _noteService.RemoveCollaboratorAsync(id, collaboratorEmail, userId);
            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}