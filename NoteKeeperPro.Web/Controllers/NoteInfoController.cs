using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteKeeperPro.Application.Dtos.NotesInfo;
using NoteKeeperPro.Application.Services.Notes;
using NoteKeeperPro.Application.Services.NotesInfo;
using NoteKeeperPro.Web.ViewModels.NotesInfo;

namespace NoteKeeperPro.Web.Controllers
{
    [Authorize]
    public class NoteInfoController : Controller
    {
        #region Services
        private readonly INoteInfoService _noteInfoService;
        private readonly INoteService _noteService; // إضافة خدمة الملاحظات
        private readonly ILogger<NoteInfoController> _logger;
        private readonly IWebHostEnvironment _env;

        public NoteInfoController(INoteInfoService noteInfoService, INoteService noteService, ILogger<NoteInfoController> logger, IWebHostEnvironment env)
        {
            _noteInfoService = noteInfoService;
            _noteService = noteService; // إضافة الخدمة للملاحظات
            _logger = logger;
            _env = env;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var noteInfos = _noteInfoService.GetAllNoteInfos();
            return View(noteInfos);
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest(); // error 400

            var noteInfo = _noteInfoService.GetNoteInfoById(id.Value);
            if (noteInfo == null)
                return NotFound(); // error 404

            return View(noteInfo);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var noteInfo = _noteInfoService.GetNoteInfoById(id.Value);
            var note = await _noteService.GetNoteByIdAsync(id.Value, null); // التأكد من الملاحظة المرتبطة

            if (noteInfo == null || note == null) return NotFound();
            return View(noteInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = _noteInfoService.DeleteNoteInfo(id); // حذف NoteInfo فقط إذا كانت موجودة
            var message = string.Empty;

            try
            {
                var noteResult = await _noteService.DeleteNoteAsync(id, null); // حذف الملاحظة نفسها
                if (noteResult)
                {
                    TempData["Message"] = "Note and Note Info Deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }
                message = "An Error Happened while Deleting";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "An Error Happened while Deleting";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));
        }
        #endregion
    }
}