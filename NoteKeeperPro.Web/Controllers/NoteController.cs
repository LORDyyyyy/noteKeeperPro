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

        #region Private Methods
        private string GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated");
            return userId;
        }
        #endregion

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            try
            {
                var userId = GetCurrentUserId();
                var notes = await _noteService.SearchNotesAsync(userId, searchTerm);
                ViewBag.SearchTerm = searchTerm;
                return View(notes);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var userId = GetCurrentUserId();
                ViewData["Notes"] = await _noteService.GetAllNotesAsync(userId);
                return View();
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteViewModel noteVM)
        {
            if (!ModelState.IsValid)
                return View(noteVM);

            try
            {
                var userId = GetCurrentUserId();
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

                ModelState.AddModelError(string.Empty, "Note Cannot be Created");
                return View(noteVM);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
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
            if (id == null)
                return BadRequest();

            try
            {
                var userId = GetCurrentUserId();
                var note = await _noteService.GetNoteByIdAsync(id.Value, userId);
                if (note == null)
                    return NotFound();

                return View(note);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                var userId = GetCurrentUserId();
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
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteViewModel noteVM)
        {
            if (!ModelState.IsValid)
                return View(noteVM);

            try
            {
                var userId = GetCurrentUserId();
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

                ModelState.AddModelError(string.Empty, "Note Cannot Be Updated");
                return View(noteVM);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
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
            if (id == null)
                return BadRequest();

            try
            {
                var userId = GetCurrentUserId();
                var note = await _noteService.GetNoteByIdAsync(id.Value, userId);
                if (note == null)
                    return NotFound();

                return View(note);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _noteService.DeleteNoteAsync(id, userId);
                if (result)
                {
                    TempData["Message"] = "Note Deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Message"] = "Note Cannot Be Deleted";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
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

        #region Recycle Bin
        [HttpGet]
        public async Task<IActionResult> RecycleBin()
        {
            try
            {
                var userId = GetCurrentUserId();
                var deletedNotes = await _noteService.GetDeletedNotesAsync(userId);
                return View(deletedNotes);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _noteService.RestoreNoteAsync(id, userId);
                if (result)
                {
                    TempData["Message"] = "Note Restored Successfully";
                }
                else
                {
                    TempData["Message"] = "Note Could Not Be Restored";
                }
                return RedirectToAction(nameof(RecycleBin));
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = HttpContext.TraceIdentifier
                };
                TempData["Message"] = "An Error Occurred While Restoring the Note";
                return View("Error", errorViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PermanentlyDelete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _noteService.PermanentlyDeleteNoteAsync(id, userId);
                if (result)
                {
                    TempData["Message"] = "Note Permanently Deleted";
                }
                else
                {
                    TempData["Message"] = "Note Could Not Be Permanently Deleted";
                }
                return RedirectToAction(nameof(RecycleBin));
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = HttpContext.TraceIdentifier
                };
                TempData["Message"] = "An Error Occurred While Deleting the Note";
                return View("Error", errorViewModel);
            }
        }
        #endregion
    }
}
