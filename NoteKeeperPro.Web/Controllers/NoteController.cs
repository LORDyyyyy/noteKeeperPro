using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteKeeperPro.Application.Dtos.Collaborators;
using NoteKeeperPro.Application.Dtos.Notes;
using NoteKeeperPro.Application.Dtos.Tags;
using NoteKeeperPro.Application.Services.Notes;
using NoteKeeperPro.Web.ViewModels.Collaborators;
using NoteKeeperPro.Web.ViewModels.Notes;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using NoteKeeperPro.Web.ViewModels.Common;
using System.Security.Claims;

namespace NoteKeeperPro.Web.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        #region Services
        private readonly INoteService _noteService;
        private readonly ILogger<NoteController> _logger;
        private readonly IWebHostEnvironment _env;

        public NoteController(INoteService noteService, ILogger<NoteController> logger, IWebHostEnvironment env)
        {
            _noteService = noteService;
            _logger = logger;
            _env = env;
        }
        #endregion

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var notes = await _noteService.SearchNotesAsync(userId, searchTerm);
            ViewBag.SearchTerm = searchTerm; // Pass the search term to the view to maintain it in the search box
            return View(notes);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            ViewData["Notes"] = await _noteService.GetAllNotesAsync(userId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteViewModel noteVM)
        {
            if (!ModelState.IsValid)
                return View(noteVM);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            string message = string.Empty;

            try
            {
                var result = await _noteService.CreateNoteAsync(new CreateNoteDto()
                {
                    Title = noteVM.Title,
                    Content = noteVM.Content,
                    TagNamesString = string.Join(",", noteVM.TagNames),
                    CollaboratorEmailsString = string.Join(",", noteVM.Collaborators.Select(c => c.UserName))
                }, userId);

                if (result != null)
                {
                    TempData["Message"] = "New Note Created Successfully";
                    return RedirectToAction(nameof(Index));
                }

                message = "Note Cannot be Created";
                TempData["Message"] = message;
                ModelState.AddModelError(string.Empty, message);
                return View(noteVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };
                return View("Error", errorViewModel);
            }
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (id == null)
                return BadRequest();

            var note = await _noteService.GetNoteByIdAsync(id.Value, userId);
            if (note == null)
                return NotFound();

            return View(note);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (id == null)
                return BadRequest();

            var note = await _noteService.GetNoteByIdAsync(id.Value, userId);
            if (note == null)
                return NotFound();

            return View(new NoteViewModel
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                TagNames = note.Tags?.Select(t => t.Name).ToList() ?? new(),
                Collaborators = note.Collaborators?
                    .Select(c => new CollaboratorViewModel
                    {
                        Id = c.Id,
                        UserName = c.UserName
                    }).ToList() ?? new()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteViewModel noteVM)
        {
            if (!ModelState.IsValid)
                return View(noteVM);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            string message = string.Empty;

            try
            {
                var result = await _noteService.UpdateNoteAsync(new UpdateNoteDto()
                {
                    Id = id,
                    Title = noteVM.Title,
                    Content = noteVM.Content,
                    TagNamesString = string.Join(",", noteVM.TagNames),
                    CollaboratorEmailsString = string.Join(",", noteVM.Collaborators.Select(c => c.UserName))
                }, userId);

                if (result != null)
                {
                    TempData["Message"] = "Note Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }

                message = "Note Cannot Be Updated";
                TempData["Message"] = message;
                ModelState.AddModelError(string.Empty, message);
                return View(noteVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = HttpContext.TraceIdentifier
                };
                TempData["Message"] = "Note Cannot Be Updated";
                return View("Error", errorViewModel);
            }
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (id is null)
                return BadRequest();

            var note = await _noteService.GetNoteByIdAsync(id.Value, userId);
            if (note == null)
                return NotFound();

            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            string message = string.Empty;
            try
            {
                var result = await _noteService.DeleteNoteAsync(id, userId);
                if (result)
                {
                    TempData["Message"] = "Note Deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }
                message = "Note Cannot Be Deleted";
                TempData["Message"] = message;
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = HttpContext.TraceIdentifier
                };
                TempData["Message"] = "Note Cannot Be Deleted";
                return View("Error", errorViewModel);
            }
        }
        #endregion
    }
}
