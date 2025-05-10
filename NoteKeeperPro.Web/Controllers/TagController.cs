using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteKeeperPro.Application.Dtos.Tags;
using NoteKeeperPro.Application.Services.Tags;
using NoteKeeperPro.Web.ViewModels.Tags;

namespace NoteKeeperPro.Web.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        #region Services
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;
        private readonly IWebHostEnvironment _env;

        public TagController(ITagService tagService, ILogger<TagController> logger, IWebHostEnvironment env)
        {
            _tagService = tagService;
            _logger = logger;
            _env = env;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var tags = _tagService.GetAllTags();
            return View(tags);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TagViewModel tagVM)
        {
            if (!ModelState.IsValid)
                return View(tagVM);

            var message = string.Empty;

            try
            {
                var result = _tagService.CreateTag(new TagToCreateDto()
                {
                    Name = tagVM.Name,
                    IsDeleted = tagVM.IsDeleted,  // إضافة الحقل IsDeleted
                    NoteIds = tagVM.NoteIds       // إضافة الحقل NoteIds
                });

                if (result > 0)
                {
                    TempData["Message"] = "New Tag Created Successfully";
                    return RedirectToAction(nameof(Index));
                }

                message = "Tag Cannot Be Created";
                TempData["Message"] = message;
                ModelState.AddModelError(string.Empty, message);
                return View(tagVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message);
                message = _env.IsDevelopment() ? ex.Message : "Tag Cannot Be Created";
                return View("Error", message);
            }
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var tag = _tagService.GetTagById(id.Value);
            if (tag == null)
                return NotFound();

            return View(tag);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var tag = _tagService.GetTagById(id.Value);
            if (tag == null)
                return NotFound();

            return View(new TagViewModel
            {
                Name = tag.Name,
                IsDeleted = tag.IsDeleted,  // إضافة الحقل IsDeleted
                NoteIds = tag.NoteIds      // إضافة الحقل NoteIds
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TagViewModel tagVM)
        {
            if (!ModelState.IsValid)
                return View(tagVM);

            var message = string.Empty;

            try
            {
                var result = _tagService.UpdateTag(new TagToUpdateDto()
                {
                    Id = id,
                    Name = tagVM.Name,
                    IsDeleted = tagVM.IsDeleted,  // إضافة الحقل IsDeleted
                    NoteIds = tagVM.NoteIds       // إضافة الحقل NoteIds
                });

                if (result > 0)
                {
                    TempData["Message"] = "Tag Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }

                message = "Tag Cannot Be Updated";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "Tag Cannot Be Updated";
            }

            ModelState.AddModelError(string.Empty, message);
            return View(tagVM);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var tag = _tagService.GetTagById(id.Value);
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;

            try
            {
                var result = _tagService.DeleteTag(id);
                if (result)
                {
                    TempData["Message"] = "Tag Deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }

                message = "Tag Cannot Be Deleted";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "An Error Occurred During Deletion";
            }

            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));
        }
        #endregion
    }
}